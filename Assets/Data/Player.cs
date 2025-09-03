using System;
using System.Collections.Generic;
using Data.Trade;

namespace Data
{
    public sealed class Player
    {
        public event Action<PlayerUpgrade> UpgradeAdded;

        public Inventory Inventory { get; }
        public List<PlayerUpgrade> Upgrades { get; } = new();

        private readonly SlotBasedInventoryPolicy _inventoryPolicy;

        public Player(int startFunds)
        {
            _inventoryPolicy = new SlotBasedInventoryPolicy();

            Inventory = new Inventory(_inventoryPolicy);
            Inventory.AddFunds(startFunds);

            AddUpgrade(PlayerUpgrade.Tier1Slots3);
        }

        // TODO: player upgrade config should define which upgrade results in which tier and how many slots they open
        public void AddUpgrade(PlayerUpgrade upgrade)
        {
            if (upgrade == PlayerUpgrade.None) return;

            Upgrades.Add(upgrade);
            UpgradeAdded?.Invoke(upgrade);

            var tier = upgrade switch
            {
                PlayerUpgrade.Tier1Slots3 => Tier.Tier1,
                PlayerUpgrade.Tier1Slots6 => Tier.Tier1,
                PlayerUpgrade.Tier1Slots9 => Tier.Tier1,
                PlayerUpgrade.Tier1Slots12 => Tier.Tier1,
                PlayerUpgrade.Tier2Slots3 => Tier.Tier2,
                PlayerUpgrade.Tier2Slots6 => Tier.Tier2,
                PlayerUpgrade.Tier3Slots3 => Tier.Tier3,
                PlayerUpgrade.Tier3Slots6 => Tier.Tier3,
            };

            _inventoryPolicy.AddSlots(tier, 3);
        }
    }
}