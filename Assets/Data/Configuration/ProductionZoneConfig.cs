using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(ProductionZoneConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(ProductionZoneConfig))]
    public sealed class ProductionZoneConfig : ScriptableObject
    {
        [field: SerializeField]
        public Color DefaultColor { get; private set; }

        [field: SerializeField]
        public Color SelectedColor { get; private set; }
    }
}