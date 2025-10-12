using Common;
using UnityEngine;

namespace Features.Towns.Development.Logic.Upgrades
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