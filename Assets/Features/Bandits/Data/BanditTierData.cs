using System;
using UnityEngine;

namespace Features.Bandits.Data
{
    /// <summary>
    /// All configurable values that scale with a bandit group's tier.
    /// </summary>
    [Serializable]
    public sealed class BanditTierData
    {
        [field: SerializeField]
        public float Health { get; private set; }

        [field: SerializeField]
        public float CombatStrength { get; private set; }

        [field: SerializeField]
        public float MovementSpeed { get; private set; }

        [field: SerializeField]
        public int HireCost { get; private set; }

        [field: SerializeField]
        public int UpgradeCost { get; private set; }

        [field: SerializeField]
        public int MaxUnitCount { get; private set; }

        [field: SerializeField]
        public int UpgradeThreshold { get; private set; }

        [field: SerializeField]
        public int RaidThreshold { get; private set; }

        [field: SerializeField]
        public int MinHirePerRestCycle { get; private set; }

        [field: SerializeField]
        public int MaxHirePerRestCycle { get; private set; }

        [field: SerializeField]
        public int CoinConsumptionPerUnitPerDay { get; private set; }

        [field: SerializeField]
        public int LootCapacityGoods { get; private set; }

        [field: SerializeField]
        public int LootCapacityCoin { get; private set; }

        [field: SerializeField]
        public int StolenGoodsPerUnit { get; private set; }

        [field: SerializeField]
        public int StolenCoinPerUnit { get; private set; }
    }
}
