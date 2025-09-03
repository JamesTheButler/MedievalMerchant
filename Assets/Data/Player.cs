using System;
using System.Collections.Generic;

namespace Data
{
    public sealed class Player
    {
        public event Action<PlayerUpgrade> UpgradeAdded;

        public Inventory Inventory { get; } = new(new UnlimitedInventoryPolicy());
        public List<PlayerUpgrade> Upgrades { get; } = new();

        public Player(int startFunds)
        {
            Inventory.AddFunds(startFunds);

            AddUpgrade(PlayerUpgrade.Tier1Slots3);
        }

        public void AddUpgrade(PlayerUpgrade upgrade)
        {
            Upgrades.Add(upgrade);
            UpgradeAdded?.Invoke(upgrade);

            // TODO: upgrade inventory policy
        }
    }
}