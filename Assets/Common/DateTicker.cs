using Common.Types;
using UnityEngine;

namespace Common
{
    public sealed class DateTicker : MonoBehaviour
    {
        [SerializeField]
        private int ticksPerDay = 1;

        private Date _date;
        private int _currentTick;

        private void Start()
        {
            _date = Model.Instance.Date;
        }

        public void Tick()
        {
            _currentTick++;
            if (_currentTick > ticksPerDay)
            {
                _date.IncrementDay();
                _currentTick = 0;
            }
        }
    }
}