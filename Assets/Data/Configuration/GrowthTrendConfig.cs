using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = "GrowthTrendConfig", menuName = AssetMenu.ConfigDataFolder + "GrowthTrendConfig")]
    public sealed class GrowthTrendConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Growth Trend", "Config")]
        public SerializedDictionary<GrowthTrend, GrowthTrendConfigData> ConfigData { get; private set; }

        public GrowthTrend GetTrend(float trend)
        {
            if (trend < ConfigData[GrowthTrend.VeryDown].Threshold) return GrowthTrend.VeryDown;
            if (trend <= ConfigData[GrowthTrend.Down].Threshold) return GrowthTrend.Down;

            if (trend > ConfigData[GrowthTrend.VeryUp].Threshold) return GrowthTrend.VeryUp;
            if (trend >= ConfigData[GrowthTrend.Up].Threshold) return GrowthTrend.Up;

            return GrowthTrend.Balanced;
        }
    }
}