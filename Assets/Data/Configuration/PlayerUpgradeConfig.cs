using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(PlayerUpgradeConfigData),
        menuName = AssetMenu.ConfigDataFolder + nameof(PlayerUpgradeConfigData))]
    public sealed class PlayerUpgradeConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Upgrade", "Data")]
        public SerializedDictionary<PlayerUpgrade, PlayerUpgradeConfigData> InventoryUpgrades { get; private set; }

        public PlayerUpgradeProgression ProgressionData { get; } = new();
    }
}