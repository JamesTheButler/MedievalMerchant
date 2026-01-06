using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Levels.GameModifiers.Events.Data;
using Features.Levels.GameModifiers.Logic;
using Features.Notifications.Logic;
using Features.Ticking;
using Features.Ticking.Logic;
using UnityEngine;

namespace Features.Levels.GameModifiers.Events
{
    public sealed class EventSystem : ISystem
    {
        private GameModifierService _gameModifierService;
        private TickingService _tickingService;
        private EventConfig _eventConfig;
        private EventModel _eventModel;
        private NotificationService _notificationService;
        private Date _gameDate;

        private const int MaxEventCreationTries = 5;

        public void Initialize()
        {
            var context = GameplayContext.Instance;
            _eventConfig = ConfigurationManager.Configurations.EventConfig;

            if (!_eventConfig.IsEnabled)
                return;

            _gameModifierService = context.Services.GameModifierService;
            _notificationService = context.Services.NotificationService;
            _tickingService = context.Services.TickingService;
            _eventModel = context.Model.Events;
            _gameDate = context.Model.Date;

            _tickingService.DayPassed += OnDayPassed;
        }

        public void CleanUp()
        {
            if (_tickingService == null)
                return;

            _tickingService.DayPassed -= OnDayPassed;
        }

        private void OnDayPassed()
        {
            RevertExpiredEvents();

            var isEventTriggered = RandomUtility.GetBool(_eventConfig.DailyEventChance);
            if (!isEventTriggered)
                return;

            if (_eventModel.OngoingEvents.Count >= _eventConfig.MaxEventCount)
                return;

            TryTriggerEvent();
        }

        private void TryTriggerEvent()
        {
            for (var i = 0; i < MaxEventCreationTries; i++)
            {
                var eventData = _eventConfig.DefaultEventSet.AvailableEvents.GetRandom();
                if (_eventModel.OngoingEvents.Any(gameEvent => gameEvent.Data == eventData))
                    continue;

                TriggerEvent(eventData);
                return;
            }
        }

        private void TriggerEvent(EventGameModifierData eventData)
        {
            var min = _eventConfig.MinDuration;
            var max = _eventConfig.MaxDuration;
            var eventDuration = Random.Range(min, max + 1); // +1 as max is exclusive
            var endDate = _gameDate + eventDuration;
            _gameModifierService.ApplyModifier(eventData, endDate);
            _notificationService.PostNotification(new EventStartedNotification(eventData));
        }

        private void RevertExpiredEvents()
        {
            var expiredEvents = _eventModel.OngoingEvents
                .Where(gameEvent => gameEvent.EndDate <= _gameDate)
                .ToList();

            foreach (var expiredEvent in expiredEvents)
            {
                _gameModifierService.RemoveModifier(expiredEvent.Data);
                _notificationService.PostNotification(new EventExpiredNotification(expiredEvent.Data));
            }
        }
    }
}