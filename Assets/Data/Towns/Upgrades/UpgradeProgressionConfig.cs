using AYellowpaper.SerializedCollections;
using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Upgrades
{
    [CreateAssetMenu(
        fileName = nameof(UpgradeProgressionConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(UpgradeProgressionConfig))]
    public sealed class UpgradeProgressionConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, UpgradeProgressionData> Progressions { get; private set; }
    }
}