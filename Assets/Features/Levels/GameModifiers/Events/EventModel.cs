using System;
using System.Collections.Generic;

namespace Features.Levels.GameModifiers.Events
{
    public sealed class EventModel
    {
        public event Action<GameEvent> EventAdded, EventRemoved;

        private readonly List<GameEvent> _ongoingEvents = new();
        public IReadOnlyList<GameEvent> OngoingEvents => _ongoingEvents;

        public void AddEvent(GameEvent gameEvent)
        {
            _ongoingEvents.Add(gameEvent);
            EventAdded?.Invoke(gameEvent);
        }

        public void RemoveEvent(GameEvent gameEvent)
        {
            _ongoingEvents.Remove(gameEvent);
            EventRemoved?.Invoke(gameEvent);
        }
    }
}