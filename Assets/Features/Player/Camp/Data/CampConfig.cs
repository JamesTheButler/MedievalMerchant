using Common.Utility;
using UnityEngine;

namespace Features.Player.Camp.Logic
{
    [CreateAssetMenu(
        fileName = nameof(CampConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(CampConfig))]
    public sealed class CampConfig : ScriptableObject
    {
        [field: SerializeField]
        public int InventorySlotCount { get; private set; } = 6;
    }
}