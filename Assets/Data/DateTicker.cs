using UnityEngine;

namespace Data
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
                _date.Day++;
                _currentTick = 0;
            }
        }
    }
}