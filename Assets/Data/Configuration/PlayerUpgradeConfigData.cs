using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = "PlayerUpgradeConfigData",
        menuName = AssetMenu.ConfigDataFolder + "PlayerUpgradeConfigData")]
    public sealed class PlayerUpgradeConfigData : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Upgrade", "Cost")]
        public SerializedDictionary<PlayerUpgrade, int> UpgradeCosts { get; private set; }

        public PlayerUpgradeProgression ProgressionData { get; } = new();
    }
}