using Common;
using UnityEngine;

namespace Features.Towns.Development.Logic.Upgrades
{
    /// <summary>
    /// When a town no longer regresses in its growth without player intervention.
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(SelfSufficienyUpgradeData),
        menuName = AssetMenu.TownUpgradesFolder + nameof(SelfSufficienyUpgradeData))]
    public sealed class SelfSufficienyUpgradeData : TownUpgradeData
    {
        public override string Description => "The town will no longer decline over time.";
    }
}