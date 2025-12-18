using Common.Infrastructure;
using Features.Player.Caravan.Logic;
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
            _player.Location.TownEntered += OnPlayerEnteredTown;
            _player.Location.TownExited += OnPlayerExitedTown;
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

        private void OnPlayerExitedTown(Town town)
        {
            caravanMiniUI.ToggleUpkeep(true);
        }

        private void OnPlayerEnteredTown(Town town)
        {
            caravanMiniUI.ToggleUpkeep(false);
        }
    }
}