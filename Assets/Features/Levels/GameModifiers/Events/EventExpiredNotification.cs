using Common.Infrastructure;
using Features.Levels.GameModifiers.Events.Data;
using Features.Notifications.Logic;

namespace Features.Levels.GameModifiers.Events
{
    public sealed record EventExpiredNotification(EventGameModifierData GameEvent) : Notification(
        GetTitle(GameEvent),
        GameEvent.Description,
        NotificationType.Info,
        Severity.Minor,
        null)
    {
        private static string GetTitle(EventGameModifierData gameEvent)
        {
            var loc = ResourceManager.Instance.LocalizationResources.NotificationResources;
            return loc.EventExpiredNotification.GetLocalizedString(gameEvent.Title);
        }
    }
}