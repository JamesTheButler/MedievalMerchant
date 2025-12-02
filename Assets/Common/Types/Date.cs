namespace Common.Types
{
    public sealed class Date
    {
        public const int LastDayOfYear = 365;

        private readonly Observable<int> _year;
        private readonly Observable<int> _day;

        public IReadOnlyObservable<int> Year => _year;
        public IReadOnlyObservable<int> Day => _day;

        public Date() : this(1, 1) { }

        public Date(int day, int year)
        {
            _year = new Observable<int>(year);
            _day = new Observable<int>(day);
        }

        public void SetDay(int day)
        {
            switch (day)
            {
                case < 1:
                    _day.Value = 1;
                    break;
                case > LastDayOfYear:
                    _day.Value = 1;
                    _year.Value++;
                    break;
                default:
                    _day.Value = day;
                    break;
            }
        }

        public static bool operator >(Date left, Date right)
        {
            if (left.Year.Value > right.Year.Value) return true;
            if (left.Year.Value < right.Year.Value) return false;
            return left.Day.Value > right.Day.Value;
        }

        public static bool operator <(Date left, Date right)
        {
            if (left.Year.Value < right.Year.Value) return true;
            if (left.Year.Value > right.Year.Value) return false;
            return left.Day.Value < right.Day.Value;
        }

        public static bool operator >=(Date left, Date right)
        {
            return !(left < right);
        }

        public static bool operator <=(Date left, Date right)
        {
            return !(left > right);
        }

        public override string ToString()
        {
            return $"Year: {_year.Value}, Day: {_day.Value}";
        }
    }
}