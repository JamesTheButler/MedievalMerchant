using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Development.Logic.Upgrades
{
    [CreateAssetMenu(
        fileName = nameof(FundsBoostUpgradeData),
        menuName = AssetMenu.TownUpgradesFolder + nameof(FundsBoostUpgradeData))]
    public sealed class FundsBoostUpgradeData : TownUpgradeData
    {
        [field: SerializeField]
        public float FundsBoost { get; private set; }
    }
}