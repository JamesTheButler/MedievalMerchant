using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Upgrades
{
    /// <summary>
    /// When a town no longer regresses in its growth without player intervention.
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(SelfSufficienyUpgradeData),
        menuName = AssetMenu.TownUpgradesFolder + nameof(SelfSufficienyUpgradeData))]
    public sealed class SelfSufficienyUpgradeData : TownUpgradeData
    {
    }
}