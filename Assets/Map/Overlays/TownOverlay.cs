using Data;
using Data.Configuration;
using Data.Towns;
using Data.Towns.Development.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Map.Overlays
{
    public sealed class TownOverlay : MonoBehaviour
    {
        [SerializeField, Required]
        private SpriteRenderer trendIcon;

        private Town _town;
        private TownDevelopmentConfig _townDevelopmentConfig;

        public void Bind(Town town)
        {
            _town = town;
            _townDevelopmentConfig = ConfigurationManager.Instance.TownDevelopmentConfig;

            transform.localPosition = town.WorldLocation;

            _town.DevelopmentManager.GrowthTrend.Observe(OnGrowthTrendChanged);
            OnGrowthTrendChanged(town.DevelopmentManager.GrowthTrend);
        }

        public void Unbind()
        {
            _town.DevelopmentManager.GrowthTrend.StopObserving(OnGrowthTrendChanged);
        }

        private void OnGrowthTrendChanged(DevelopmentTrend trend)
        {
            trendIcon.sprite = trend == DevelopmentTrend.Balanced
                ? null
                : _townDevelopmentConfig.GrowthTrendConfig[trend].Icon;
        }
    }
}