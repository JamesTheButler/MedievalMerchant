using Features.Levels.GameModifiers.Events.Data;
using Features.Localization.Data;
using Features.Notifications.Logic;
using UnityEngine.ResourceManagement;
using ResourceManager = Common.Infrastructure.ResourceManager;

namespace Features.Levels.GameModifiers.Events
{
    public sealed record EventStartedNotification(EventGameModifierData GameEvent) : Notification(
        GetTitle(GameEvent),
        $"{GameEvent.Description}\n\n{GameEvent.EffectsString}",
        NotificationType.Info,
        Severity.Major,
        null)
    {
        private static string GetTitle(EventGameModifierData gameEvent)
        {
            var loc = ResourceManager.Instance.LocalizationResources.Notifications;
            return loc.EventStartedNotification.GetLocalizedString(gameEvent.Title);
        }
    }
}