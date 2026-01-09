using Common.Infrastructure;
using Common.UI;
using Features.Ticking.Logic;
using Features.Towns;

namespace Features.Player.Logic
{
    public sealed class PlayerInTownPauseSystem : ISystem
    {
        private PlayerLocation _playerLocation;
        private GameSpeedModel _gameSpeedModel;
        private UIBridgeService _uiBridgeService;

        public void Initialize()
        {
            _playerLocation = GameplayContext.Instance.Model.Player.Location;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _uiBridgeService = GameplayContext.Instance.Services.UIBridgeService;

            _playerLocation.CurrentTown.Observe(OnTownChanged);
            _uiBridgeService.NavigationStarted += OnNavigationStarted;
        }

        public void CleanUp()
        {
            _playerLocation.CurrentTown.StopObserving(OnTownChanged);
            _uiBridgeService.NavigationStarted -= OnNavigationStarted;
        }

        private void OnTownChanged(Town town)
        {
            if (town == null) return;

            _gameSpeedModel.Pause();
        }

        private void OnNavigationStarted(Town town)
        {
            _gameSpeedModel.Resume();
        }
    }
}