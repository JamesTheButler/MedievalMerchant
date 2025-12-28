using Common.Infrastructure;
using Common.Types;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed record MissionStartedNotification : Notification
    {
        public MissionStartedNotification(Town town, Mission mission) : base(
            GetTitle(town, mission),
            GetDescription(mission),
            NotificationType.Info,
            GetIcon(mission.Good)) { }

        private static string GetTitle(Town town, Mission mission)
        {
            var config = ResourceManager.Instance.GoodsResources.ResourceData[mission.Good];
            return $"Mission started: {town.Name} wants {config.GoodName}.";
        }

        private static string GetDescription(Mission mission)
        {
            var config = ResourceManager.Instance.GoodsResources.ResourceData[mission.Good];
            return $"Deliver {mission.TotalCount}x {config.GoodName} before {mission.EndDate}.";
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodsResources.ResourceData[missionGood];
            return configData.Icon;
        }
    }
}