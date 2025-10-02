using AYellowpaper.SerializedCollections;
using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Upgrades
{
    [CreateAssetMenu(
        fileName = nameof(UpgradeProgressionData),
        menuName = AssetMenu.ConfigDataFolder + nameof(UpgradeProgressionData))]
    public sealed class UpgradeProgressionData : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<float, TownUpgradeData[]> Upgrades { get; private set; }
    }
}