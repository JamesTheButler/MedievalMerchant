using System;
using System.Linq;
using Common;
using Common.Types;
using Features.Towns.Development.Config;
using Features.Towns.Development.Logic;
using NaughtyAttributes;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace Features.Towns.Development.UI.DevelopmentGauge
{
    public sealed class DevelopmentGauge : MonoBehaviour
    {
        [SerializeField, Required]
        private DevelopmentSlider developmentSlider;

        [SerializeField, Required]
        private TMP_Text developmentTrendText;

        [SerializeField, Required]
        private Image developmentTrendIcon;

        [SerializeField, Required]
        private TooltipHandler developmentTrendTooltip;

        private readonly Lazy<TownDevelopmentConfig> _townDevelopmentConfig =
            new(() => ConfigurationManager.Instance.TownDevelopmentConfig);

        private Town _town;
        private DevelopmentManager _developmentManager;

        public void Bind(Town town)
        {
            _town = town;
            _developmentManager = _town.DevelopmentManager;

            town.Tier.Observe(OnTierChanged);

            _developmentManager.DevelopmentScore.Observe(UpdateDevelopmentScore);
            _developmentManager.DevelopmentTrend.Observe(UpdateDevelopmentTrend);
            _developmentManager.GrowthTrend.Observe(UpdateGrowthTrend);
        }

        private void OnTierChanged(Tier tier)
        {
            developmentSlider.ClearMilestones();

            var upgrades = _townDevelopmentConfig.Value.Milestones[tier].MilestoneData;

            var milestones = upgrades
                .Select(pair => new DevelopmentMilestone.Data(pair.Key, pair.Value.Icon, pair.Value.Description))
                .ToList();

            developmentSlider.SetMilestones(milestones);
        }

        public void Unbind()
        {
            developmentSlider.ClearMilestones();

            _developmentManager.DevelopmentScore.StopObserving(UpdateDevelopmentScore);
            _developmentManager.DevelopmentTrend.StopObserving(UpdateDevelopmentTrend);
            _developmentManager.GrowthTrend.StopObserving(UpdateGrowthTrend);
        }

        private void UpdateGrowthModifierTooltip()
        {
            var trend = _developmentManager.DevelopmentTrend;
            var modifiers = trend.Modifiers;
            developmentTrendTooltip.SetEnabled(modifiers.Any());
            developmentTrendTooltip.SetTooltip(trend.ToString());
        }

        private void UpdateDevelopmentScore(float score)
        {
            developmentSlider.SetDevelopment(score);
        }

        private void UpdateDevelopmentTrend(float trend)
        {
            var sign = trend > 0 ? "+" : "";
            developmentTrendText.text = $"{sign}{trend}%";

            UpdateGrowthModifierTooltip();
        }

        private void UpdateGrowthTrend(DevelopmentTrend trend)
        {
            developmentTrendIcon.sprite = _townDevelopmentConfig.Value.GrowthTrendConfig[trend].Icon;
        }
    }
}