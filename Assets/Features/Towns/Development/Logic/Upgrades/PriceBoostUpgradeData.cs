using Common;
using UnityEngine;

namespace Features.Towns.Development.Logic.Upgrades
{
    [CreateAssetMenu(
        fileName = nameof(PriceBoostUpgradeData),
        menuName = AssetMenu.TownUpgradesFolder + nameof(PriceBoostUpgradeData))]
    public sealed class PriceBoostUpgradeData : TownUpgradeData
    {
        [field: SerializeField]
        public float PriceBoostPercent { get; private set; }

        public override string Description => $"Improves the towns prices by {PriceBoostPercent.ToPercentString()}.";
    }
}