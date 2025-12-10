using System;
using Common;
using UnityEngine;

namespace Features.Ticking
{
    public class FloatBasedTicker : ITicker
    {
        public float ValueRatePerDay { get; set; }

        private readonly Action<float> _tickAction;
        private int _ticksPerDay;

        private bool _isInitialized;

        public FloatBasedTicker(Action<float> tickAction, float valueRatePerDay)
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
            _isInitialized = true;
        }

        public void Tick()
        {
            if (!_isInitialized)
            {
                Debug.LogWarning("Trying to tick an uninitialized ticker.");
                return;
            }

            _tickAction?.Invoke(ValueRatePerDay / _ticksPerDay);
        }
    }
}