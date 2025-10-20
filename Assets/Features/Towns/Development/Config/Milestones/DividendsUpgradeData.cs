using Common;
using UnityEngine;

namespace Features.Towns.Development.Config.Milestones
{
    /// <summary>
    /// Upgrade for automatically transferring part of the towns fund-production to the player.
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(DividendsUpgradeData),
        menuName = AssetMenu.TownUpgradesFolder + nameof(DividendsUpgradeData))]
    public sealed class DividendsUpgradeData : TownUpgradeData
    {
        [field: SerializeField]
        public float DividendsPercentage { get; private set; }
        
        public override string Description => $"Receive {DividendsPercentage.ToPercentString()} of the towns coin production.";
    }
}