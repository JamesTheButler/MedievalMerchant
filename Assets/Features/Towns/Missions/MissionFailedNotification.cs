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
            GetTitle(town, mission),
            GetDescription(town, mission),
            NotificationType.Bad,
            Severity.Minor,
            GetIcon(mission.Good))
        {
            Town = town;
        }

        private static string GetTitle(Town town, Mission mission)
        {
            var loc = ResourceManager.Instance.LocalizationResources.MissionStrings;
            return mission.Type == MissionType.TradeMission
                ? loc.GetTradeMissionFailedTitle(town.Name)
                : loc.GetUpgradeMissionFailedTitle(town.Name);
        }

        private static string GetDescription(Town town, Mission mission)
        {
            var loc = ResourceManager.Instance.LocalizationResources.MissionStrings;
            var goodName = ResourceManager.Instance.GoodResources.ResourceData[mission.Good].GoodName;
            return loc.GetMissionFailedDescription(town.Name, goodName);
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodResources.ResourceData[missionGood];
            return configData.Icon;
        }
    }
}