using Common.Infrastructure;
using Common.Types;
using Features.Notifications.Logic;
using UnityEngine;

namespace Features.Towns.Missions
{
    public sealed record MissionFailedNotification : Notification
    {
        public MissionFailedNotification(Town town, Mission mission)
            : base(GetTitle(town, mission), GetDescription(town, mission), GetIcon(mission.Good)) { }

        private static string GetTitle(Town town, Mission mission)
        {
            var config = ResourceManager.Instance.GoodsResources.ConfigData[mission.Good];
            return $"Mission failed: {town.Name} no longer wants {config.GoodName}.";
        }

        private static string GetDescription(Town town, Mission mission)
        {
            var config = ResourceManager.Instance.GoodsResources.ConfigData[mission.Good];
            return $"You did not deliver {config.name} in time. {town.Name} is not happy.";
        }

        private static Sprite GetIcon(Good missionGood)
        {
            var configData = ResourceManager.Instance.GoodsResources.ConfigData[missionGood];
            return configData.Icon;
        }
    }
}