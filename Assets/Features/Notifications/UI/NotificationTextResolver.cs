using Common.Infrastructure;
using Features.Levels.Conditions.Model;
using Features.Levels.GameModifiers.Events;
using Features.Localization.Data;
using Features.Notifications.Logic;
using Features.Towns.Missions;

namespace Features.Notifications.UI
{
    public sealed class NotificationTextResolver
    {
        private readonly NotificationLocalizationResources _notifLocalization = ResourceManager.Instance.LocalizationResources.NotificationResources;

        public string GetTitle(Notification notification)
        {
            return notification switch
            {
                LossConditionNotification =>
                    _notifLocalization.LossImminentNotificationTitle.GetLocalizedString(),
                EventExpiredNotification notif =>
                    _notifLocalization.EventExpiredNotification.GetLocalizedString(notif.GameEvent.Title),
                EventStartedNotification notif =>
                    _notifLocalization.EventStartedNotification.GetLocalizedString(notif.GameEvent.Title),
                MissionFailedNotification notif =>
                    _notifLocalization.MissionFailedNotification.GetLocalizedString(notif.Town.Name),
                MissionStartedNotification notif =>
                    GetMissionStartedTitle(notif),
            };
        }

        private string GetMissionStartedTitle(MissionStartedNotification notif)
        {
            var formatter = notif.Mission.Type switch
            {
                MissionType.TradeMission => _notifLocalization.TradeMissionStartedNotification,
                MissionType.UpgradeMission => _notifLocalization.UpgradeMissionStartedNotification,
            };

            return formatter.GetLocalizedString(notif.Town.Name);
        }
    }
}