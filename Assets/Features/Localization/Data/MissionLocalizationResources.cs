using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class MissionLocalizationResources
    {
        [SerializeField]
        private LocalizedString tradeMissionCoinReward,
            tradeMissionGrowthReward,
            tradeMissionReputationReward,
            tradeMissionGrowthPenalty,
            tradeMissionReputationPenalty,
            upgradeMissionReputationReward,
            upgradeMissionGrowthPenalty,
            upgradeMissionReputationPenalty,
            missionStartedTitle,
            missionStartedDescription,
            missionFailedTitle,
            missionFailedDescription;

        public string TradeMissionCoinReward(float value)
        {
            return ResolveMissionDetailString(tradeMissionCoinReward, value);
        }

        public string TradeMissionGrowthReward(float value)
        {
            return ResolveMissionDetailString(tradeMissionGrowthReward, value);
        }

        public string TradeMissionReputationReward(float value)
        {
            return ResolveMissionDetailString(tradeMissionReputationReward, value);
        }

        public string TradeMissionGrowthPenalty(float value)
        {
            return ResolveMissionDetailString(tradeMissionGrowthPenalty, value);
        }

        public string TradeMissionReputationPenalty(float value)
        {
            return ResolveMissionDetailString(tradeMissionReputationPenalty, value);
        }

        public string UpgradeMissionGrowthPenalty(float value)
        {
            return ResolveMissionDetailString(upgradeMissionGrowthPenalty, value);
        }

        public string UpgradeMissionReputationReward(float value)
        {
            return ResolveMissionDetailString(upgradeMissionReputationReward, value);
        }

        public string UpgradeMissionReputationPenalty(float value)
        {
            return ResolveMissionDetailString(upgradeMissionReputationPenalty, value);
        }

        public string GetMissionStartedTitle(string townName)
        {
            return missionStartedTitle.GetLocalizedString(new { TownName = townName });
        }

        public string GetMissionStartedDescription(string townName, string goodName, int goodAmount)
        {
            return missionStartedTitle.GetLocalizedString(new
            {
                TownName = townName,
                GoodName = goodName,
                _int_Amount = goodAmount
            });
        }

        public string GetMissionFailedTitle(string townName)
        {
            return missionFailedTitle.GetLocalizedString(new { TownName = townName });
        }

        public string GetMissionFailedDescription(string townName)
        {
            return missionFailedTitle.GetLocalizedString(new { TownName = townName });
        }

        private string ResolveMissionDetailString(LocalizedString s, float value)
        {
            return s.GetLocalizedString(value);
        }
    }
}