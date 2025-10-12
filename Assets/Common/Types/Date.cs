namespace Common.Types
{
    public sealed class Date
    {
        public const int LastDayOfYear = 365;

        private readonly Observable<int> _year;
        private readonly Observable<int> _day;

        public IReadOnlyObservable<int> Year => _year;
        public IReadOnlyObservable<int> Day => _day;

        public Date() : this(1, 1)
        {
        }

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
    }
}