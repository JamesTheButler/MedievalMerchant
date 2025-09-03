using System;
using System.Collections.Generic;

namespace Data
{
    public sealed class Player
    {
        public Inventory Inventory { get; private set; } = new();

        public List<PlayerUpgrade> Upgrades { get; private set; } = new();

        public event Action<PlayerUpgrade> UpgradeAdded;

        public Player(int startFunds)
        {
            Inventory.AddFunds(startFunds);

            AddUpgrade(PlayerUpgrade.Tier1Slots3);
            AddUpgrade(PlayerUpgrade.Tier3Slots3);
        }

        public void AddUpgrade(PlayerUpgrade upgrade)
        {
            Upgrades.Add(upgrade);
            UpgradeAdded?.Invoke(upgrade);

            // TODO: upgrade inventory policy
        }
    }
}