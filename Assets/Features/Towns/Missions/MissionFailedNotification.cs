using Common.Infrastructure;
using Common.Types;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed record MissionFailedNotification : Notification
    {
        public Town Town { get; }

        public MissionFailedNotification(Town town, Mission mission) : base(
            GetTitle(town),
            GetDescription(town, mission),
            NotificationType.Bad,
            Severity.Minor,
            GetIcon(mission.Good))
        {
            Town = town;
        }

        private static string GetTitle(Town town)
        {
            var loc = ResourceManager.Instance.LocalizationResources.NotificationResources;
            return loc.MissionFailedNotification.GetLocalizedString(town.Name);
        }

        private static string GetDescription(Town town, Mission mission)
        {
            var config = ResourceManager.Instance.GoodResources.ResourceData[mission.Good];
            return $"You did not deliver enough {config.GoodName} in time. {town.Name} is not happy.";
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodResources.ResourceData[missionGood];
            return configData.Icon;
        }
    }
}