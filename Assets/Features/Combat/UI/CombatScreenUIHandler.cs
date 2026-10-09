using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Features.Bandits.Logic;
using Features.Combat.Logic;
using Features.Player.Logic;
using Features.Player.Retinue;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Combat.UI
{
    public sealed class CombatScreenUIHandler : InitializableBehavior
    {
        [SerializeField, Required]
        private CombatScreenUI combatScreen;

        private CombatService _combatService;
        private PlayerModel _player;

        private readonly Bindings _bindings = new();

        public override void Initialize()
        {
            _combatService = GameplayContext.Instance.Services.CombatService;
            _player = GameplayContext.Instance.Model.Player;

            _bindings.Track(
                combatScreen.RoundRequested.Observe(OnRoundRequested),
                combatScreen.OutcomeDismissed.Observe(OnOutcomeDismissed)
            );

            combatScreen.Hide();
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _bindings.Unbind();
        }

        public void StartBattle(BanditGang bandits)
        {
            var captain = _player.RetinueModel.Companions[CompanionType.Guard];
            var playerCombatant = _combatService.GetPlayerCombatant(captain.Level.Value);
            var banditCombatant = _combatService.GetBanditCombatant(bandits);
            var combat = _combatService.StartBattle(playerCombatant, banditCombatant);
            combatScreen.Show(combat);
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
            // TODO: route to the loot screen for this outcome.
            combatScreen.Hide();
        }
    }
}