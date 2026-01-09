using Common.Infrastructure;
using Features.Ticking.Logic;
using Features.Towns;

namespace Features.Player.Logic
{
    public sealed class PlayerInTownPauseSystem : ISystem
    {
        private PlayerLocation _playerLocation;
        private GameSpeedModel _gameSpeedModel;

        public void Initialize()
        {
            _playerLocation = GameplayContext.Instance.Model.Player.Location;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;

            //_playerLocation.CurrentTown.Observe(OnTownChanged);
        }

        public void CleanUp()
        {
            //_playerLocation.CurrentTown.StopObserving(OnTownChanged);
        }

        private void OnTownChanged(Town town)
        {
            if (town == null) return;

            _gameSpeedModel.Pause();
        }
    }
}