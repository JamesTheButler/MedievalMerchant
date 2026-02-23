using Features.Levels.GameModifiers.Events.Data;
using Features.Notifications.Logic;

namespace Features.Levels.GameModifiers.Events
{
    public sealed record EventStartedNotification(EventGameModifierData GameEvent) : Notification(
        $"{GameEvent.Description}\n\n{GameEvent.EffectsString}",
        NotificationType.Info,
        Severity.Major,
        null);
}