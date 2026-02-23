using Features.Levels.GameModifiers.Events.Data;
using Features.Notifications.Logic;

namespace Features.Levels.GameModifiers.Events
{
    public sealed record EventExpiredNotification(EventGameModifierData GameEvent) : Notification(
        GameEvent.Description,
        NotificationType.Info,
        Severity.Minor,
        null);
}