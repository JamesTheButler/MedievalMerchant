using UnityEngine;
using UnityEngine.Events;

namespace Common
{
    public sealed class GameTicker : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent onTick;

        private float _timer;
        private bool _isTicking;
        private float _secondsPerTick;

        public void StartTicking()
        {
            _isTicking = true;
        }

        public void StopTicking()
        {
            _isTicking = false;
        }

        private void Start()
        {
            _secondsPerTick = ConfigurationManager.Configurations.TickConfig.SecondsPerTick;
        }

        private void FixedUpdate()
        {
            if (!_isTicking) return;

            _timer += Time.fixedDeltaTime;

            while (_timer >= _secondsPerTick)
            {
                _timer -= _secondsPerTick;
                onTick?.Invoke();
            }
        }
    }
}