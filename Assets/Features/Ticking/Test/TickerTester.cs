using UnityEngine;

namespace Features.Ticking.Test
{
    public sealed class TickerTester : MonoBehaviour
    {
        [SerializeField]
        private float floatRate = 0.2f, intRate = 24;

        [SerializeField]
        private bool testFloat, testInt;

        [SerializeField]
        private int ticksPerDay = 10;

        [SerializeField]
        private int internalTicksPerSec = 5;

        private int _internalTick;

        private IntBasedTicker _intTicker;
        private FloatBasedTicker _floatTicker;

        private int _intTick, _floatTick;
        private int _intSum;
        private float _floatSum;

        private void Awake()
        {
            _intTicker = new IntBasedTicker(IntTick, intRate);
            _intTicker.Initialize(ticksPerDay);
            _floatTicker = new FloatBasedTicker(FloatTick, floatRate);
            _intTicker.Initialize(ticksPerDay);
        }

        private void FixedUpdate()
        {
            _internalTick++;
            if (_internalTick < internalTicksPerSec) return;

            if (_internalTick > 100) return;

            if (testInt)
            {
                _intTicker.Tick();
            }

            if (testFloat)
            {
                _floatTicker.Tick();
            }
        }

        private void IntTick(int valueChange)
        {
            _intTick++;
            _intSum += valueChange;
            Debug.LogError($"INT {_intTick}: +{valueChange} -- {_intSum}");
        }

        private void FloatTick(float valueChange)
        {
            _floatTick++;
            _floatSum += valueChange;
            Debug.LogError($"FLOAT {_floatTick}: +{valueChange} -- {_floatSum}");
        }
    }
}