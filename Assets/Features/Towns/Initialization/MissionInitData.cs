using System;
using Common.Infrastructure;
using Common.Types;
using Features.Towns.Missions;
using UnityEngine;

namespace Features.Towns.Initialization
{
    [Serializable]
    public sealed class MissionInitData : InitData
    {
        [SerializeField]
        private Good good;

        [SerializeField]
        private int amount;

        public override void Initialize(Town town)
        {
            var tradeMissionConfig = ConfigurationManager.Configurations.MissionConfig.TradeMissionData;

            town.Missions.AddMission(new Mission(
                good,
                amount,
                new Date(1, 1),
                new Date(90, 1),
                MissionType.TradeMission,
                tradeMissionConfig.GetReward(),
                tradeMissionConfig.GetPenalty()));
        }
    }
}