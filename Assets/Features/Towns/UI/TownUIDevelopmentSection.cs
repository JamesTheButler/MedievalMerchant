using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
using Common.UI.Utility;
using Common.Utility;
using Features.Towns.Development.Config;
using Features.Towns.Development.Logic;
using Features.Towns.Development.UI.DevelopmentGauge;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.UI
{
    public sealed class TownUIDevelopmentSection : TownUISection
    {
        [SerializeField, Required]
        private DevelopmentSlider developmentSlider;

        [SerializeField, Required]
        private TMP_Text developmentTrendText;

        [SerializeField, Required]
        private Image developmentTrendIcon;

        [SerializeField, Required]
        private SimpleTooltipHandler developmentTooltip;

        [SerializeField, Required]
        private ModifiableTooltipHandler developmentTrendTooltip;

        private TownDevelopmentConfig _townDevelopmentConfig;

        private Town _town;
        private DevelopmentManager _developmentManager;

        public override void Initialize()
        {
            _townDevelopmentConfig = ConfigurationManager.Configurations.TownDevelopmentConfig;
        }

        public override void Bind(Town town)
        {
            _town = town;
            _developmentManager = _town.DevelopmentManager;

            town.Tier.Observe(OnTierChanged);

            _developmentManager.DevelopmentScore.Observe(UpdateDevelopmentScore);
            _developmentManager.DevelopmentTrend.Observe(UpdateDevelopmentTrend);
            _developmentManager.GrowthTrend.Observe(UpdateGrowthTrend);
        }

        public override void Unbind(Town town)
        {
            developmentSlider.ClearMilestones();

            _developmentManager.DevelopmentScore.StopObserving(UpdateDevelopmentScore);
            _developmentManager.DevelopmentTrend.StopObserving(UpdateDevelopmentTrend);
            _developmentManager.GrowthTrend.StopObserving(UpdateGrowthTrend);
        }

        public override void CleanUp() { }

        private void OnTierChanged(Tier tier)
        {
            developmentSlider.ClearMilestones();

            var upgrades = _townDevelopmentConfig.Milestones[tier].MilestoneData;

            var milestones = upgrades
                .Select(pair => new DevelopmentMilestone.Data(pair.Key, pair.Value.Icon, pair.Value.Description))
                .ToList();

            developmentSlider.SetMilestones(milestones);
        }

        private void UpdateGrowthModifierTooltip()
        {
            var trend = _developmentManager.DevelopmentTrend;
            var modifiers = trend.Modifiers;
            developmentTrendTooltip.SetEnabled(modifiers.Any());
            developmentTrendTooltip.SetData(trend);
        }

        private void UpdateDevelopmentScore(float score)
        {
            developmentSlider.SetDevelopment(score);
            developmentTooltip.SetData($"Development: {score:0.#}");
        }

        private void UpdateDevelopmentTrend(float trend)
        {
            var style = trend.GetNumberStyle();
            developmentTrendText.text = $"{trend.Sign()}{trend:0.##}/day".WithStyle(style);

            UpdateGrowthModifierTooltip();
        }

        private void UpdateGrowthTrend(DevelopmentTrend trend)
        {
            developmentTrendIcon.sprite = _townDevelopmentConfig.GrowthTrendConfig[trend].Icon;
        }
    }
}