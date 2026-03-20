using System;
using Common.Types;
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
            tradeMissionStartedTitle,
            upgradeMissionStartedTitle,
            missionStartedDescription,
            tradeMissionFailedTitle,
            upgradeMissionFailedTitle,
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

        public string GetTradeMissionStartedTitle(string townName, string goodName)
        {
            var args = new
            {
                TownName = townName,
                GoodName = goodName
            };
            return tradeMissionStartedTitle.GetLocalizedString(args);
        }

        public string GetUpgradeMissionStartedTitle(string townName, string goodName)
        {
            var args = new
            {
                TownName = townName,
                GoodName = goodName
            };
            return upgradeMissionStartedTitle.GetLocalizedString(args);
        }

        public string GetMissionStartedDescription(string goodName, int goodAmount, Date date)
        {
            return missionStartedDescription.GetLocalizedString(new
            {
                GoodName = goodName,
                _int_Amount = goodAmount,
                Date = date.ToDisplayString()
            });
        }

        public string GetTradeMissionFailedTitle(string townName)
        {
            return tradeMissionFailedTitle.GetLocalizedString(new { TownName = townName });
        }

        public string GetUpgradeMissionFailedTitle(string townName)
        {
            return upgradeMissionFailedTitle.GetLocalizedString(new { TownName = townName });
        }

        public string GetMissionFailedDescription(string townName, string goodName)
        {
            var args = new
            {
                TownName = townName,
                GoodName = goodName
            };
            return missionFailedDescription.GetLocalizedString(args);
        }

        private string ResolveMissionDetailString(LocalizedString s, float value)
        {
            return s.GetLocalizedString(value);
        }
    }
}