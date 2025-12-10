using System;
using UnityEngine;

namespace Features.Ticking
{
    public class IntBasedTicker : ITicker
    {
        private readonly Action<int> _tickAction;

        /// Rate that is applied each tick.
        private int _flatRatePerDay;

        /// Rate that is used every few ticks to fill up the remainder of the total daily tick rate.
        private float _fractionRatePerDay;

        // Remainder of the value change that is accumulated across ticks and is applied once it spills over 1.
        private float _remainingValue;

        private float _valueRatePerDay;
        private int _ticksPerDay;
        private bool _isInitialized;

        public float ValueRatePerDay
        {
            set
            {
                _valueRatePerDay = value;
                RecalculateRates();
            }
        }

        public IntBasedTicker(Action<int> tickAction, float valueRatePerDay)
        {
            _tickAction = tickAction;
            ValueRatePerDay = valueRatePerDay;
        }

        public void Initialize(int ticksPerDay)
        {
            if (_isInitialized)
            {
                Debug.LogError("Ticker has already been set up. Changing tick rates on live ticker is not supported.");
                return;
            }

            _ticksPerDay = ticksPerDay;
            RecalculateRates();

            _isInitialized = true;
        }

        public void Tick()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("Trying to tick an uninitialized ticker.");
                return;
            }

            _remainingValue += _fractionRatePerDay;

            var fractionValue = 0;
            if (_remainingValue >= 1f)
            {
                _remainingValue -= 1;
                fractionValue = 1;
            }

            _tickAction?.Invoke(_flatRatePerDay + fractionValue);
        }

        private void RecalculateRates()
        {
            // no reason to calculate these yet, as we don't know all the setup info yet
            if (!_isInitialized)
                return;

            var totalRatePerTick = _valueRatePerDay / _ticksPerDay;
            _flatRatePerDay = Mathf.FloorToInt(totalRatePerTick);
            _fractionRatePerDay = totalRatePerTick - _flatRatePerDay;
        }
    }
}