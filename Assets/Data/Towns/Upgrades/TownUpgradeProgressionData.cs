using AYellowpaper.SerializedCollections;
using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Upgrades
{
    [CreateAssetMenu(
        fileName = nameof(TownUpgradeProgressionData),
        menuName = AssetMenu.ConfigDataFolder + nameof(TownUpgradeProgressionData))]
    public sealed class TownUpgradeProgressionData : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<float, TownUpgradeData[]> Upgrades { get; private set; }
    }
}