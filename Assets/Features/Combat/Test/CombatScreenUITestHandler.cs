using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Features.Bandits.Logic;
using Features.Combat.Logic;
using Features.Combat.UI;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Combat.Test
{
    public sealed class CombatScreenUITestHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private CombatScreenUI combatScreen;

        [SerializeField]
        private bool runOnStart = true;

        [SerializeField]
        private int captainLevel = 3;

        [SerializeField]
        private BanditTier banditTier = BanditTier.Tier2;

        [SerializeField]
        private int banditUnitCount = 5;

        [SerializeField]
        private BanditBehaviorState banditState = BanditBehaviorState.Raiding;

        private readonly CombatService _combatService = new();
        private readonly Bindings _bindings = new();

        private void Awake()
        {
            _combatService.Initialize();

            _bindings.Track(
                combatScreen.RoundRequested.Observe(OnRoundRequested),
                combatScreen.OutcomeDismissed.Observe(OnOutcomeDismissed)
            );

            combatScreen.Hide();
        }

        private void Start()
        {
            if (runOnStart)
            {
                Run();
            }
        }

        private void OnDestroy()
        {
            _bindings.Unbind();
            _combatService.CleanUp();
        }

        [Button("Run Combat", EButtonEnableMode.Playmode)]
        private void Run()
        {
            combatScreen.Hide();

            var player = _combatService.GetPlayerCombatant(captainLevel);
            var bandits = _combatService.GetBanditCombatant(BuildGang());

            ApplyStateEffect(bandits);

            combatScreen.Show(_combatService.StartBattle(player, bandits));
        }

        private BanditGang BuildGang()
        {
            var banditConfig = ConfigurationManager.Configurations.BanditConfig;
            var tierData = banditConfig.GetTierData(banditTier);

            var gang = new BanditGang();
            gang.Tier.Value = banditTier;
            gang.UnitCount.Value = banditUnitCount > 0 ? banditUnitCount : tierData.MaxUnitCount;
            gang.UnitHealth.Value = tierData.Health;
            gang.UnitCombatStrength.Value = tierData.CombatStrength;

            return gang;
        }

        private void ApplyStateEffect(Combatant bandits)
        {
            var banditConfig = ConfigurationManager.Configurations.BanditConfig;
            var effect = banditConfig.GetStateEffect(banditState).StrengthEffect;

            if (Mathf.Approximately(effect, 0f))
                return;

            bandits.UnitCombatStrength.AddModifier(
                new CombatStateModifier(effect, banditState.ToString()));
        }

        private void OnRoundRequested()
        {
            var result = _combatService.ResolveRound();

            if (result == null)
                return;

            combatScreen.PlayRound(result);
        }

        private void OnOutcomeDismissed()
        {
            combatScreen.Hide();
        }
    }
}