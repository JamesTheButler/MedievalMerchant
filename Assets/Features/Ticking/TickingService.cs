using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Config;

namespace Features.Ticking
{
    public sealed class TickingService : IService
    {
        public event Action DayPassed;

        public bool IsPaused { get; private set; }

        private readonly HashSet<ITicker> _tickers = new();
        private TickConfig _tickConfig;

        private int _dayTicks;
        private float _timer;
        private float _secondsPerTick;
        private int _ticksPerDay;

        public void Initialize()
        {
            _tickers.Clear();
            _tickConfig = ConfigurationManager.Configurations.TickConfig;
            _ticksPerDay = _tickConfig.TicksPerDay;
            _secondsPerTick = _tickConfig.SecondsPerTick;
        }

        public void CleanUp()
        {
            _tickers.Clear();
        }

        public void Update(float deltaTime)
        {
            if (IsPaused) return;

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

        public void Pause()
        {
            IsPaused = true;
        }

        public void Resume()
        {
            IsPaused = false;
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