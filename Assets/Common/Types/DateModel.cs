using System;
using Common.Infrastructure.Observation;

namespace Common.Types
{
    public sealed class DateModel
    {
        public const int LastDayOfYear = 365;

        private readonly Observable<Date> _gameDate = new(new Date());
        public IReadOnlyObservable<Date> GameDate => _gameDate;

        public void Increment()
        {
            _gameDate.Value++;
        }

        public void SetDay(int day)
        {
            _gameDate.Value = _gameDate.Value with { Day = Math.Clamp(day, 1, LastDayOfYear) };
        }
    }
}