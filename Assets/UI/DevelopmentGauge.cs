using System;
using System.Linq;
using Data;
using Data.Configuration;
using Data.Towns;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public sealed class DevelopmentGauge : MonoBehaviour
    {
        [SerializeField, Required]
        private DevelopmentSlider developmentScore;

        [SerializeField, Required]
        private TMP_Text developmentTrendText;

        [SerializeField, Required]
        private Image developmentTrendIcon;

        [SerializeField, Required]
        private TooltipHandler developmentTrendTooltip;

        private readonly Lazy<TownDevelopmentConfig> _growthConfig =
            new(() => ConfigurationManager.Instance.TownDevelopmentConfig);

        private DevelopmentManager _developmentManager;

        public void Bind(DevelopmentManager developmentManager)
        {
            _developmentManager = developmentManager;

            developmentManager.DevelopmentTrend.ModifiersChanged += UpdateGrowthModifierTooltip;
            developmentManager.DevelopmentScore.Observe(UpdateDevelopmentScore);
            developmentManager.DevelopmentTrend.Observe(UpdateDevelopmentTrend);
            developmentManager.GrowthTrend.Observe(UpdateGrowthTrend);

            UpdateGrowthModifierTooltip();
        }

        public void Unbind()
        {
            _developmentManager.DevelopmentTrend.ModifiersChanged -= UpdateGrowthModifierTooltip;
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
            developmentScore.SetDevelopment(score);
        }

        private void UpdateDevelopmentTrend(float trend)
        {
            var sign = trend > 0 ? "+" : "";
            developmentTrendText.text = $"{sign}{trend}%";
        }

        private void UpdateGrowthTrend(DevelopmentTrend obj)
        {
            developmentTrendIcon.sprite = _growthConfig.Value.GrowthTrendConfig[obj].Icon;
        }
    }
}