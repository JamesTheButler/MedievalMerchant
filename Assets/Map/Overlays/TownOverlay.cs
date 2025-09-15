using Data;
using Data.Configuration;
using Data.Towns;
using NaughtyAttributes;
using UnityEngine;

namespace Map.Overlays
{
    public sealed class TownOverlay : MonoBehaviour
    {
        [SerializeField, Required]
        private SpriteRenderer trendIcon;

        private Town _town;
        private GrowthTrendConfig _growthTrendConfig;

        public void Bind(Town town)
        {
            _town = town;
            var model = Model.Instance;
            _growthTrendConfig = ConfigurationManager.Instance.GrowthTrendConfig;

            transform.localPosition = town.WorldLocation;

            _town.GrowthTrend.Observe(OnGrowthTrendChanged);
            OnGrowthTrendChanged(town.GrowthTrend);
        }

        public void Unbind()
        {
            _town.GrowthTrend.StopObserving(OnGrowthTrendChanged);
        }

        private void OnGrowthTrendChanged(GrowthTrend trend)
        {
            trendIcon.sprite = trend == GrowthTrend.Balanced
                ? null
                : _growthTrendConfig.ConfigData[trend].Icon;
        }
    }
}