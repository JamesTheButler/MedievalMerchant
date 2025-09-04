using System;

namespace Data
{
    public sealed class Date
    {
        private int _year = 1;
        private int _day = 1;

        public event Action<int> YearChanged;
        public event Action<int> DayChanged;

        public int Year
        {
            get => _year;
            private set
            {
                _year = value;
                YearChanged?.Invoke(_year);
            }
        }

        public int Day
        {
            get => _day;
            set
            {
                if (value > 365)
                {
                    _day = 1;
                    Year++;
                }
                else
                {
                    _day = value;
                }

                DayChanged?.Invoke(_day);
            }
        }
    }
}