using Common.Types;
using Infrastructure;
using UnityEngine;

namespace Common
{
    public sealed class DateTicker : MonoBehaviour
    {
        private Date _date;
        private int _ticksPerDay;
        private int _currentTick;

        private void Start()
        {
            _date = GameplayContext.Instance.Model.Date;
            _ticksPerDay = ConfigurationManager.Configurations.TickConfig.TicksPerDay;
        }

        public void Tick()
        {
            if (++_currentTick <= _ticksPerDay)
                return;

            _date.IncrementDay();
            _currentTick = 0;
        }
    }
}