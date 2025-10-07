using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Production.Config
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