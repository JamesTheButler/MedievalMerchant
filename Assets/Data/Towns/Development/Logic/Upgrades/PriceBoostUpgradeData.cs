using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Development.Logic.Upgrades
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