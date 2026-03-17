using Common.Infrastructure;
using Common.Types;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed record MissionStartedNotification(Town Town, Mission Mission) : Notification(
        GetTitle(town, mission),
        GetDescription(Mission),
        NotificationType.Info,
        Mission.Type == MissionType.TradeMission ? Severity.Minor : Severity.Major,
        GetIcon(Mission.Good))
    {
        private static string GetTitle(object town, object mission)
        {
            
        }

        private static string GetDescription(Mission mission)
        {
            var loc = ResourceManager.Instance.LocalizationResources.MissionStrings;
            var config = ResourceManager.Instance.GoodResources.ResourceData[mission.Good];
            return $"Deliver {mission.TotalCount}x {config.GoodName} before {mission.EndDate}.";
            return loc.GetMissionStartedDescription()
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodResources.ResourceData[missionGood];
            return configData.Icon;
        }
    }
}