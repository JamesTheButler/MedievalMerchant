using System;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Towns.Development.Config;
using Features.Towns.Missions.Results;
using Features.Towns.Reputation.Data;
using NaughtyAttributes;
using TMPro;
using UnityEngine;

namespace Features.Towns.Missions.UI
{
    public sealed class MissionTooltip : TooltipBase<Mission>
    {
        [SerializeField, Required]
        private Sprite coinIcon;

        [SerializeField, Required]
        private TMP_Text titleText, descriptionText;

        [SerializeField, Required]
        private RectTransform rewardGroup, penaltyGroup;

        [SerializeField, Required]
        private TextWithIconElement detailItemPrefab;

        private TownDevelopmentConfig _townDevelopmentConfig;
        private ReputationResources _reputationResources;

        protected override void Awake()
        {
            base.Awake();

            _townDevelopmentConfig = ConfigurationManager.Configurations.TownDevelopmentConfig;
            _reputationResources = ResourceManager.Instance.ReputationResources;
        }

        protected override void UpdateUI(Mission data)
        {
            Reset();
            titleText.text = data.Type == MissionType.TradeMission ? "Trade Mission" : "Upgrade Mission";
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
            var happyIcon = _reputationResources.UnhappyIcon;
            var growthTrendData = _townDevelopmentConfig.GrowthTrendConfig;

            switch (result)
            {
                case TradeMissionPenalty tradePenalty:
                    var trendDownIcon = growthTrendData[DevelopmentTrend.Down].Icon;
                    AddResultDetailItem($"{tradePenalty.GrowthPenalty} growth", trendDownIcon, penaltyGroup);
                    AddResultDetailItem($"{tradePenalty.ReputationPenalty} reputation", unhappyIcon, penaltyGroup);
                    break;
                case TradeMissionReward tradeReward:
                    var trendUpIcon = growthTrendData[DevelopmentTrend.Up].Icon;
                    AddResultDetailItem($"{tradeReward.Coin} coin", coinIcon, rewardGroup);
                    AddResultDetailItem($"{tradeReward.Growth} growth", trendUpIcon, rewardGroup);
                    AddResultDetailItem($"{tradeReward.Reputation} reputation", happyIcon, rewardGroup);
                    break;
                case UpgradeMissionPenalty upgradePenalty:
                    var trendVeryDownIcon = growthTrendData[DevelopmentTrend.VeryDown].Icon;
                    AddResultDetailItem($"{upgradePenalty.GrowthPenalty} growth", trendVeryDownIcon, penaltyGroup);
                    AddResultDetailItem($"{upgradePenalty.ReputationPenalty} reputation", unhappyIcon, penaltyGroup);
                    break;
                case UpgradeMissionReward upgradeReward:
                    AddResultDetailItem($"{upgradeReward.ReputationReward} reputation", happyIcon, rewardGroup);
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