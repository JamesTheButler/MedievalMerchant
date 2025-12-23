using System.Collections.Generic;
using Common.Types;
using Features.Levels.GameModifiers.Events.Data;

namespace Features.Levels.GameModifiers.Events
{
    public sealed class EventModel
    {
        public readonly Dictionary<EventGameModifierData, Date> OngoingEvents = new();
    }
}