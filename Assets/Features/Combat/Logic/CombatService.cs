using System.Collections.Generic;
using System.Linq;
using Common.Config.Sampling;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Utility;
using Features.Localization.Data;
using Features.Player.Logic;
using Features.Player.Retinue;
using UnityEngine;

namespace Features.Combat.Logic
{
    public sealed class CombatService : IService
    {
        // TODO: Config
        // TODO: better Samplers
        private const float PlayerHitFactorMin = 0.92f, PlayerHitFactorMax = 1.18f;
        private const float BanditHitFactorMin = 0.40f, BanditHitFactorMax = 1.60f;

        public Combat OngoingBattle { get; private set; }

        private PlayerModel _player;
        private CombatLocalizationResources _loc;

        public void Initialize()
        {
            _player = GameplayContext.Instance.Model.Player;
            _loc = ResourceManager.Instance.LocalizationResources.Combat;
        }

        public void CleanUp()
        {
            OngoingBattle = null;
        }

        private Combatant GetPlayerCombatant()
        {
            var captain = _player.RetinueModel.Companions[CompanionType.Guard];
            return new Combatant(
                level: captain.Level.Value,
                unitCount: 0,
                baseHealth: 0f,
                baseCombatStrength: 0f,
                healthDescription: "",
                combatStrengthDescription: "",
                hitSampler: new UniformSampler(PlayerHitFactorMin, PlayerHitFactorMax));
        }

        // TODO: missing type for bandit gang
        private Combatant GetBanditCombatant(object banditGang)
        {
            return new Combatant(
                level: 0,
                unitCount: 0,
                baseHealth: 0f,
                baseCombatStrength: 0f,
                healthDescription: "",
                combatStrengthDescription: "",
                hitSampler: new UniformSampler(BanditHitFactorMin, BanditHitFactorMax));
        }

        public Combat StartBattle(Combatant player, Combatant bandits)
        {
            OngoingBattle = new Combat(GetPlayerCombatant(), bandits);
            return OngoingBattle;
        }

        public RoundResult ResolveRound()
        {
            var combat = OngoingBattle;
            if (combat == null || combat.IsOver)
                return null;

            var guardsBefore = Snapshot(combat.Player);
            var banditsBefore = Snapshot(combat.Bandits);

            var attacks = new List<Attack>();
            attacks.AddRange(CollectAttacks(combat.Player, combat.Bandits));
            attacks.AddRange(CollectAttacks(combat.Bandits, combat.Player));

            var aliveBefore = new HashSet<CombatUnit>(
                combat.Player.AliveUnits.Concat(combat.Bandits.AliveUnits));

            foreach (var attack in attacks)
            {
                attack.Defender.ReceiveDamage(attack.Damage);
            }

            var fallen = aliveBefore.Where(unit => !unit.IsAlive.Value).ToList();

            combat.RoundCounter.Value++;

            var result = new RoundResult
            {
                Round = combat.RoundCounter.Value,
                Attacks = attacks,
                Fallen = fallen,
                Guards = DeltaSince(guardsBefore, combat.Player),
                Bandits = DeltaSince(banditsBefore, combat.Bandits),
                Status = ResolveCombatStatus(combat),
            };

            return result;
        }

        private static CombatStatus ResolveCombatStatus(Combat combat)
        {
            return (combat.Player.IsAlive, combat.Bandits.IsAlive) switch
            {
                (true, true) => CombatStatus.Ongoing,
                (true, false) => CombatStatus.Victory,
                (false, true) => CombatStatus.Defeat,
                _ => CombatStatus.Draw,
            };
        }

        private static ICollection<Attack> CollectAttacks(Combatant attackers, Combatant defenders)
        {
            var attacks = new List<Attack>();
            var targets = defenders.AliveUnits.ToList();
            if (targets.Count == 0)
                return attacks;

            var strength = attackers.UnitCombatStrength.Value;

            foreach (var attacker in attackers.AliveUnits)
            {
                // TODO: pick the target by proximity to the attacker rather than at random.
                var target = targets.GetRandom();
                attacks.Add(new Attack(attacker, target, strength * attackers.HitSampler.Sample()));
            }

            return attacks;
        }

        private static (int alive, float health, float strength) Snapshot(Combatant combatant) =>
            (combatant.AliveCount.Value, combatant.TotalHealth.Value, combatant.TotalCombatStrength.Value);

        private static CombatantDelta DeltaSince((int alive, float health, float strength) before, Combatant now) =>
            new(
                before.alive - now.AliveCount.Value,
                before.health - now.TotalHealth.Value,
                before.strength - now.TotalCombatStrength.Value);
    }
}