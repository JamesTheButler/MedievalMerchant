using Common.Infrastructure;
using Common.Types;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed record MissionStartedNotification(Town Town, Mission Mission) : Notification(
        GetDescription(Mission),
        NotificationType.Info,
        Mission.Type == MissionType.TradeMission ? Severity.Minor : Severity.Major,
        GetIcon(Mission.Good))
    {
        private static string GetDescription(Mission mission)
        {
            var config = ResourceManager.Instance.GoodResources.ResourceData[mission.Good];
            return $"Deliver {mission.TotalCount}x {config.GoodName} before {mission.EndDate}.";
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodResources.ResourceData[missionGood];
            return configData.Icon;
        }
    }
}