using AYellowpaper.SerializedCollections;
using Data.Player;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(PlayerUpgradeConfigData),
        menuName = AssetMenu.ConfigDataFolder + nameof(PlayerUpgradeConfigData))]
    public sealed class PlayerConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Upgrade", "Data")]
        public SerializedDictionary<PlayerUpgrade, PlayerUpgradeConfigData> InventoryUpgrades { get; private set; }

        public PlayerUpgradeProgression ProgressionData { get; } = new();

        [field: SerializeField]
        public float MovementSpeed { get; private set; }
    }
}