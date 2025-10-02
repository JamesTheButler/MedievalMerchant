using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Upgrades
{
    [CreateAssetMenu(
        fileName = nameof(PriceBoostUpgradeData),
        menuName = AssetMenu.TownUpgradesFolder + nameof(PriceBoostUpgradeData))]
    public sealed class PriceBoostUpgradeData : TownUpgradeData
    {
        [field: SerializeField]
        public float PriceBoostPercent { get; private set; }
    }
}