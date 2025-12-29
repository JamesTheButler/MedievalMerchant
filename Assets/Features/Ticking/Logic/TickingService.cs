using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Features.Ticking.Config;

namespace Features.Ticking.Logic
{
    public sealed class TickingService : IService
    {
        public event Action DayPassed;

        private readonly HashSet<ITicker> _tickers = new();
        private TickConfig _tickConfig;
        private GameSpeedModel _gameSpeedModel;

        private int _dayTicks;
        private float _timer;
        private float _secondsPerTick;
        private int _ticksPerDay;

        public void Initialize()
        {
            _tickers.Clear();
            _tickConfig = ConfigurationManager.Configurations.TickConfig;
            _ticksPerDay = _tickConfig.TicksPerDay;
            _gameSpeedModel = GameplayContext.Instance.Model.GameSpeed;
            _gameSpeedModel.GameSpeed.Observe(OnGameSpeedChanged);
        }

        public void CleanUp()
        {
            _tickers.Clear();
        }

        public void Update(float deltaTime)
        {
            if (_gameSpeedModel.IsPaused.Value)
                return;

            _timer += deltaTime;

            while (_timer >= _secondsPerTick)
            {
                _timer -= _secondsPerTick;
                Tick();
            }
        }

        public void RegisterTicker(ITicker ticker)
        {
            ticker.Initialize(_ticksPerDay);
            _tickers.Add(ticker);
        }

        public void UnregisterTicker(ITicker ticker)
        {
            _tickers.Remove(ticker);
        }

        private void OnGameSpeedChanged(GameSpeed speed)
        {
            var secPerDay = speed switch
            {
                GameSpeed.Normal => _tickConfig.SecondsPerDayDefault,
                GameSpeed.Fast => _tickConfig.SecondsPerDayFast,
                _ => _tickConfig.SecondsPerDayDefault
            };
            _secondsPerTick = secPerDay / _ticksPerDay;
        }

        private void Tick()
        {
            _dayTicks++;
            if (_dayTicks >= _ticksPerDay)
            {
                _dayTicks = 0;
                DayPassed?.Invoke();
            }

            // .toArray() avoids CollectionModifiedExceptions
            foreach (var ticker in _tickers.ToArray())
            {
                ticker.Tick();
            }
        }
    }
}