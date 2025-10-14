using Common;
using UnityEngine;

namespace Features.Towns.Development.Logic.Upgrades
{
    [CreateAssetMenu(
        fileName = nameof(FundsBoostUpgradeData),
        menuName = AssetMenu.TownUpgradesFolder + nameof(FundsBoostUpgradeData))]
    public sealed class FundsBoostUpgradeData : TownUpgradeData
    {
        [field: SerializeField]
        public float FundsBoost { get; private set; }
        
        public override string Description => $"Increases the towns coin production by {FundsBoost.ToPercentString()}.";
    }
}