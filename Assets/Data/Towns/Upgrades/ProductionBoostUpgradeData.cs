using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Upgrades
{
    [CreateAssetMenu(
        fileName = nameof(ProductionBoostUpgradeData),
        menuName = AssetMenu.TownUpgradesFolder + nameof(ProductionBoostUpgradeData))]
    public sealed class ProductionBoostUpgradeData : TownUpgradeData
    {
        [field: SerializeField]
        public float ProductionBoost { get; private set; }
    }
}