using System;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Levels.GameModifiers.Events.Data;

namespace Features.Levels.GameModifiers.Events
{
    public sealed class GameEvent
    {
        public EventGameModifierData Data { get; }
        public Date EndDate { get; }
        public Observable<int> DaysLeft { get; } = new();

        public GameEvent(EventGameModifierData eventData, Date endDate)
        {
            Data = eventData;
            EndDate = endDate;
        }

        public void UpdateGameDate(Date gameDate)
        {
            var dayDiff = EndDate.AsDays() - gameDate.AsDays();
            DaysLeft.Value = Math.Max(0, dayDiff);
        }
    }
}