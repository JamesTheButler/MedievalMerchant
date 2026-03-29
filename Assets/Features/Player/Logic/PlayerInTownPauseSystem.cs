using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Features.Map;
using Features.Map.Pathfinding;
using Features.Ticking.Logic;

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
                _playerLocation.MapLocation.Observe(OnLocationChanged),
                _navigationService.NavigationStarted.Observe(OnNavigationStarted)
            );
        }

        public void CleanUp()
        {
            _bindings.UnbindAll();
        }

        private void OnLocationChanged(IMapLocation location)
        {
            if (location == null) return;

            _gameSpeedModel.Pause();
        }

        private void OnNavigationStarted(IMapLocation location)
        {
            _gameSpeedModel.Resume();
        }
    }
}