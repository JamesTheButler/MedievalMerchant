using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Player.Caravan.Logic;
using Features.Player.Logic;
using Features.Towns;
using UnityEngine;

namespace Features.Player.Caravan.UI
{
    public sealed class CaravanMiniUIHandler : MonoBehaviour
    {
        [SerializeField]
        private CaravanMiniUI caravanMiniUI;

        private PlayerModel _player;
        private CaravanManager _caravanManager;

        private void Start()
        {
            _player = GameplayContext.Instance.Model.Player;
            _player.Location.CurrentTown.Observe(OnPlayerEnteredTown);

            _caravanManager = _player.CaravanManager;
            _caravanManager.MoveSpeed.Observe(OnMoveSpeedChanged);
            _caravanManager.Upkeep.Observe(OnUpkeepChanged);
        }

        private void OnUpkeepChanged(float upkeep)
        {
            caravanMiniUI.SetUpkeep(upkeep);
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            caravanMiniUI.SetMoveSpeed(moveSpeed);
        }

        private void OnPlayerEnteredTown(Town town)
        {
            // toggle when player leaves town
            caravanMiniUI.ToggleUpkeep(town == null);
        }
    }
}