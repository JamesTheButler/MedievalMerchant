using System;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Localization.Data;
using Features.Towns.Development.Config;
using Features.Towns.Missions.Results;
using Features.Towns.Reputation.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Towns.Missions.UI
{
    public sealed class MissionTooltip : TooltipBase<Mission>
    {
        [SerializeField, Required]
        private Sprite coinIcon;

        [SerializeField]
        private LocalizedString tradeMissionTitle, upgradeMissionTitle;

        [SerializeField, Required]
        private TMP_Text titleText, descriptionText;

        [SerializeField, Required]
        private RectTransform rewardGroup, penaltyGroup;

        [SerializeField, Required]
        private TextWithIconElement detailItemPrefab;

        private MissionLocalizationResources _loc;
        private TownDevelopmentConfig _townDevelopmentConfig;
        private ReputationResources _reputationResources;

        protected override void Awake()
        {
            base.Awake();

            _loc = ResourceManager.Instance.LocalizationResources.MissionStrings;
            _townDevelopmentConfig = ConfigurationManager.Configurations.TownDevelopmentConfig;
            _reputationResources = ResourceManager.Instance.ReputationResources;
        }

        protected override void UpdateUI(Mission data)
        {
            Reset();
            var titleString = data.Type == MissionType.TradeMission ? tradeMissionTitle : upgradeMissionTitle;
            titleText.text = titleString.GetLocalizedString();
            descriptionText.gameObject.SetActive(data.Type == MissionType.UpgradeMission);

            RenderResult(data.Reward);
            RenderResult(data.Penalty);
        }

        public override void Reset()
        {
            rewardGroup.DestroyChildren();
            penaltyGroup.DestroyChildren();
        }

        private void RenderResult(IMissionResult result)
        {
            var unhappyIcon = _reputationResources.UnhappyIcon;
            var happyIcon = _reputationResources.HappyIcon;
            var growthTrendData = _townDevelopmentConfig.GrowthTrendConfig;

            switch (result)
            {
                case TradeMissionPenalty penalty:
                    var trendDownIcon = growthTrendData[DevelopmentTrend.Down].Icon;
                    AddResultDetailItem(_loc.TradeMissionGrowthPenalty(penalty.Growth), trendDownIcon, penaltyGroup);
                    AddResultDetailItem(_loc.TradeMissionReputationPenalty(penalty.Reputation), unhappyIcon, penaltyGroup);
                    break;
                case TradeMissionReward reward:
                    var trendUpIcon = growthTrendData[DevelopmentTrend.Up].Icon;
                    AddResultDetailItem(_loc.TradeMissionCoinReward(reward.Coin), coinIcon, rewardGroup);
                    AddResultDetailItem(_loc.TradeMissionGrowthReward(reward.Growth), trendUpIcon, rewardGroup);
                    AddResultDetailItem(_loc.TradeMissionReputationReward(reward.Reputation), happyIcon, rewardGroup);
                    break;
                case UpgradeMissionPenalty penalty:
                    var trendVeryDownIcon = growthTrendData[DevelopmentTrend.VeryDown].Icon;
                    AddResultDetailItem(_loc.UpgradeMissionGrowthPenalty(penalty.Growth), trendVeryDownIcon, penaltyGroup);
                    AddResultDetailItem(_loc.UpgradeMissionReputationPenalty(penalty.Reputation), unhappyIcon, penaltyGroup);
                    break;
                case UpgradeMissionReward reward:
                    AddResultDetailItem(_loc.UpgradeMissionReputationReward(reward.ReputationReward), happyIcon, rewardGroup);
                    break;
                default: throw new ArgumentOutOfRangeException(nameof(result));
            }
        }

        private void AddResultDetailItem(string text, Sprite icon, RectTransform container)
        {
            var item = Instantiate(detailItemPrefab, container);
            item.SetUp(text, icon);
        }
    }
}