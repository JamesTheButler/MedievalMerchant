using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(ProducerConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(ProducerConfig))]
    public sealed class ProducerConfig : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Building Tier", "Cost")]
        private SerializedDictionary<Tier, int[]> upgradeCosts;

        public int? GetUpgradeCost(Tier tier, int buildingIndex)
        {
            if (buildingIndex > upgradeCosts[tier].Length) return null;
            return upgradeCosts[tier][buildingIndex];
        }
    }
}