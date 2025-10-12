using Data.Configuration;
using UnityEngine;

namespace Data.Towns.Development.Logic.Upgrades
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
    }
}