using System.Collections.Generic;
using System.Linq;
using Common.Config.Sampling;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Utility;
using Features.Bandits.Data;
using Features.Combat.Data;
using Features.Bandits.Logic;
using Features.Localization.Data;
using Features.Player.Logic;
using Features.Player.Retinue;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Config.Resources;
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
        private CombatConfig _config;
        private CompanionResources _companionResources;
        private BanditResources _banditResources;
        private CombatLocalizationResources _loc;

        public void Initialize()
        {
            _player = GameplayContext.Instance.Model.Player;
            _config = ConfigurationManager.Configurations.CombatConfig;
            _loc = ResourceManager.Instance.LocalizationResources.Combat;
            _companionResources = ResourceManager.Instance.CompanionResources;
            _banditResources = ResourceManager.Instance.BanditResources;
        }

        public void CleanUp()
        {
            OngoingBattle = null;
        }

        private Combatant GetPlayerCombatant()
        {
            var captain = _player.RetinueModel.Companions[CompanionType.Guard];
            return new Combatant(
                "Player", // @claude, localize
                level: captain.Level.Value,
                unitCount: 0,
                unitHealth: 0f,
                unitCombatStrength: 0f,
                healthDescription: "",
                combatStrengthDescription: "",
                hitSampler: new UniformSampler(PlayerHitFactorMin, PlayerHitFactorMax),
                commanderIcon: _companionResources.Guard.Icon,
                unitIcon: _companionResources.GuardIcon);
        }

        private Combatant GetBanditCombatant(BanditGang banditGang)
        {
            return new Combatant(
                "Bandit", // @claude, localize
                level: (int)banditGang.Tier.Value,
                unitCount: banditGang.UnitCount,
                unitHealth: banditGang.UnitHealth,
                unitCombatStrength: banditGang.UnitCombatStrength,
                healthDescription: "",
                combatStrengthDescription: "",
                hitSampler: new UniformSampler(BanditHitFactorMin, BanditHitFactorMax),
                commanderIcon: _banditResources.BanditCommanderIcon,
                unitIcon: _banditResources.BanditUnitIcon);
        }

        public Combat StartBattle(BanditGang bandits)
        {
            OngoingBattle = new Combat(GetPlayerCombatant(), GetBanditCombatant(bandits));
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

            var status = ResolveCombatStatus(combat);
            var share = combat.GuardHealthShare.Value;

            var result = new RoundResult
            {
                Round = combat.RoundCounter.Value,
                Attacks = attacks,
                Fallen = fallen,
                Guards = DeltaSince(guardsBefore, combat.Player),
                Bandits = DeltaSince(banditsBefore, combat.Bandits),
                Status = status,
                PlayerMood = ResolveMood(status, share, isPlayer: true),
                BanditMood = ResolveMood(status, share, isPlayer: false),
            };

            return result;
        }

        // Both sides are read off the same status and health share, so they can never
        // contradict each other.
        private CombatMood ResolveMood(CombatStatus status, float guardHealthShare, bool isPlayer)
        {
            switch (status)
            {
                case CombatStatus.Victory:
                    return isPlayer ? CombatMood.Won : CombatMood.Lost;
                case CombatStatus.Defeat:
                    return isPlayer ? CombatMood.Lost : CombatMood.Won;
                case CombatStatus.Draw:
                    return CombatMood.Lost;
            }

            var share = isPlayer ? guardHealthShare : 1f - guardHealthShare;
            var lead = (share - 0.5f) * 2f;

            if (lead > _config.AheadMoodLead)
                return CombatMood.Ahead;

            return lead < -_config.AheadMoodLead ? CombatMood.Behind : CombatMood.Even;
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