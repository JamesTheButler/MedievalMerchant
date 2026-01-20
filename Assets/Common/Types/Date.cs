using Common.Infrastructure.Observation;

namespace Common.Types
{
    public sealed class Date
    {
        public const int LastDayOfYear = 365;

        public ObservableEvent<Date> Changed { get; } = new();

        private readonly Observable<int> _year;
        private readonly Observable<int> _day;

        public IReadOnlyObservable<int> Year => _year;
        public IReadOnlyObservable<int> Day => _day;

        public Date() : this(1, 1) { }

        public Date(int day, int year)
        {
            _year = new Observable<int>(year);
            _day = new Observable<int>();
            SetDay(day);
        }

        public void SetDay(int day)
        {
            // NOTE: this does not support operations where day is > 365 * 2
            switch (day)
            {
                case < 1:
                    _day.Value = LastDayOfYear - day;
                    _year.Value--;
                    break;
                case > LastDayOfYear:
                    _day.Value = day - LastDayOfYear;
                    _year.Value++;
                    break;
                default:
                    _day.Value = day;
                    break;
            }

            Changed?.Invoke(this);
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

        public static Date operator +(Date left, Date right)
        {
            return new Date(left.Day.Value + right.Day.Value, left.Year.Value + right.Year.Value);
        }

        public static Date operator -(Date left, Date right)
        {
            return new Date(left.Day.Value - right.Day.Value, left.Year.Value - right.Year.Value);
        }

        public static Date operator +(Date left, int days)
        {
            return new Date(left.Day.Value + days, left.Year.Value);
        }

        public override string ToString()
        {
            return $"Year: {_year.Value}, Day: {_day.Value}";
        }

        public string ToDisplayString()
        {
            return $"Day {_day.Value} of Year {_year.Value}";
        }
    }
}