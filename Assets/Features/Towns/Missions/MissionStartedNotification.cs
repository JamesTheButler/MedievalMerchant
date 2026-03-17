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
            return loc.GetMissionStartedTitle(town.Name);
        }

        private static string GetDescription(Town town, Mission mission)
        {
            var loc = ResourceManager.Instance.LocalizationResources.MissionStrings;
            var goodConfig = ResourceManager.Instance.GoodResources.ResourceData[mission.Good];
            return loc.GetMissionStartedDescription(town.Name, goodConfig.GoodName, mission.TotalCount);
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodResources.ResourceData[missionGood];
            return configData.Icon;
        }
    }
}