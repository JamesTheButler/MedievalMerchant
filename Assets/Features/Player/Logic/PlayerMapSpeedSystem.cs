using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Features.Player.Caravan.Config;
using Features.Ticking.Config;
using Features.Ticking.Logic;

namespace Features.Player.Logic
{
    public sealed class PlayerMapSpeedSystem : ISystem
    {
        private PlayerModel _player;
        private TickConfig _tickConfig;
        private CaravanConfig _caravanConfig;
        private GameSpeedModel _gameSpeedModel;

        private float _currentMoveSpeed, _currentSecPerDay;

        public void Initialize()
        {
            _tickConfig = ConfigurationManager.Configurations.TickConfig;
            _caravanConfig = ConfigurationManager.Configurations.CaravanConfig;
            _player = GameplayContext.Instance.Model.Player;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;

            _player.MovementSpeed.Observe(OnMoveSpeedChanged);
            _gameSpeedModel.GameSpeed.Observe(OnGameSpeedChanged);
        }

        public void CleanUp()
        {
            _player.MovementSpeed.StopObserving(OnMoveSpeedChanged);
            _gameSpeedModel.GameSpeed.StopObserving(OnGameSpeedChanged);
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            _currentMoveSpeed = moveSpeed;
            RecalculateMapSpeed();
        }

        private void OnGameSpeedChanged(GameSpeed gameSpeed)
        {
            _currentSecPerDay = gameSpeed == GameSpeed.Normal
                ? _tickConfig.SecondsPerDayDefault
                : _tickConfig.SecondsPerDayFast;
            RecalculateMapSpeed();
        }

        private void RecalculateMapSpeed()
        {
            var tilesPerTicksPerDay =
                _caravanConfig.TilesPerMoveSpeedPointPerDay * _currentMoveSpeed / _currentSecPerDay;
            _player.SpeedInTilesPerDay.Value = tilesPerTicksPerDay;
        }
    }
}