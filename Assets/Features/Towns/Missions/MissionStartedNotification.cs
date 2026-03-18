using Common.Infrastructure;
using Common.Types;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed record MissionStartedNotification(Town Town, Mission Mission) : Notification(
        GetTitle(Town, Mission),
        GetDescription(Town, Mission),
        NotificationType.Info,
        Mission.Type == MissionType.TradeMission ? Severity.Minor : Severity.Major,
        GetIcon(Mission.Good))
    {
        private static string GetTitle(Town town, Mission mission)
        {
            var loc = ResourceManager.Instance.LocalizationResources.MissionStrings;
            var goodName = ResourceManager.Instance.GoodResources.ResourceData[mission.Good].GoodName;
            return mission.Type == MissionType.TradeMission
                ? loc.GetTradeMissionStartedTitle(town.Name, goodName)
                : loc.GetUpgradeMissionStartedTitle(town.Name, goodName);
        }

        private static string GetDescription(Town town, Mission mission)
        {
            var loc = ResourceManager.Instance.LocalizationResources.MissionStrings;
            var goodName = ResourceManager.Instance.GoodResources.ResourceData[mission.Good].GoodName;
            return loc.GetMissionStartedDescription(goodName, mission.TotalCount, mission.EndDate);
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodResources.ResourceData[missionGood];
            return configData.Icon;
        }
    }
}