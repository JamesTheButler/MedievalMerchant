using AYellowpaper.SerializedCollections;
using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Development.Config
{
    [CreateAssetMenu(
        fileName = nameof(TownDevelopmentConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(TownDevelopmentConfig))]
    public sealed class TownDevelopmentConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Growth Trend", "Config")]
        public SerializedDictionary<DevelopmentTrend, GrowthTrendConfigData> GrowthTrendConfig { get; private set; }

        [field: SerializeField]
        public TownGoodConfigTable<float> ProducerGrowthInfluence { get; private set; }

        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, TownDevelopmentTable> DevelopmentTables { get; private set; }
        
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, DevelopmentMilestoneDataSet> Milestones { get; private set; }

        [field: SerializeField]
        public float DevelopmentMultiplier { get; private set; } = 1f;

        public DevelopmentTrend GetTrend(float trend)
        {
            if (trend < GrowthTrendConfig[DevelopmentTrend.VeryDown].Threshold) return DevelopmentTrend.VeryDown;
            if (trend <= GrowthTrendConfig[DevelopmentTrend.Down].Threshold) return DevelopmentTrend.Down;

            if (trend > GrowthTrendConfig[DevelopmentTrend.VeryUp].Threshold) return DevelopmentTrend.VeryUp;
            if (trend >= GrowthTrendConfig[DevelopmentTrend.Up].Threshold) return DevelopmentTrend.Up;

            return DevelopmentTrend.Balanced;
        }
    }
}