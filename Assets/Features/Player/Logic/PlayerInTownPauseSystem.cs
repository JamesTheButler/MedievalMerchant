using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Common.UI;
using Features.Map;
using Features.Ticking.Logic;
using Features.Towns;

namespace Features.Player.Logic
{
    public sealed class PlayerInTownPauseSystem : ISystem
    {
        private readonly Bindings _bindings = new();

        private PlayerLocation _playerLocation;
        private GameSpeedModel _gameSpeedModel;
        private NavigationService _navigationService;

        public void Initialize()
        {
            _playerLocation = GameplayContext.Instance.Model.Player.Location;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _navigationService = GameplayContext.Instance.Services.NavigationService;

            _bindings.Track(
                _playerLocation.CurrentTown.Observe(OnTownChanged),
                _navigationService.NavigationStarted.Observe(OnNavigationStarted)
            );
        }

        public void CleanUp()
        {
            _bindings.UnbindAll();
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