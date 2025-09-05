using Common;

namespace Data
{
    public sealed class Date
    {
        private readonly Observable<int> _year = new(1);
        private readonly Observable<int> _day = new(1);

        public IReadOnlyObservable<int> Year => _year;
        public IReadOnlyObservable<int> Day => _day;

        public void SetDay(int day)
        {
            switch (day)
            {
                case < 1:
                    _day.Value = 1;
                    break;
                case > 365:
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