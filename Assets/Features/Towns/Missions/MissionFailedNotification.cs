using Common.Infrastructure;
using Common.Types;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed record MissionFailedNotification : Notification
    {
        public MissionFailedNotification(Town town, Mission mission) : base(
            GetTitle(town, mission),
            GetDescription(town, mission),
            NotificationType.Bad,
            Severity.Minor,
            GetIcon(mission.Good)) { }

        private static string GetTitle(Town town, Mission mission)
        {
            var config = ResourceManager.Instance.GoodsResources.ResourceData[mission.Good];
            return $"Mission failed: {town.Name}";
        }

        private static string GetDescription(Town town, Mission mission)
        {
            var config = ResourceManager.Instance.GoodsResources.ResourceData[mission.Good];
            return $"You did not deliver enough {config.GoodName} in time. {town.Name} is not happy.";
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodsResources.ResourceData[missionGood];
            return configData.Icon;
        }
    }
}