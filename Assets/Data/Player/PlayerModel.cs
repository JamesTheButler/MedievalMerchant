using System;
using System.Collections.Generic;
using Data.Modifiable;
using Data.Player.Retinue;
using Data.Trade;

namespace Data.Player
{
    public sealed class PlayerModel
    {
        public event Action<PlayerUpgrade> UpgradeAdded;

        public List<PlayerUpgrade> Upgrades { get; } = new();
        public PlayerLocation Location { get; } = new();
        
        public ModifiableVariable MovementSpeed { get; }
        public Inventory Inventory { get; }
        public RetinueManager RetinueManager { get; }
        
        private readonly SlotBasedInventoryPolicy _inventoryPolicy;

        public PlayerModel(int startFunds, float movementSpeed)
        {
            _inventoryPolicy = new SlotBasedInventoryPolicy();
            MovementSpeed = new ModifiableVariable(new BaseMovementSpeedModifier(movementSpeed));
            RetinueManager = new RetinueManager();
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

            Tier? tier = upgrade switch
            {
                PlayerUpgrade.Tier1Slots3 => Tier.Tier1,
                PlayerUpgrade.Tier1Slots6 => Tier.Tier1,
                PlayerUpgrade.Tier1Slots9 => Tier.Tier1,
                PlayerUpgrade.Tier1Slots12 => Tier.Tier1,
                PlayerUpgrade.Tier2Slots3 => Tier.Tier2,
                PlayerUpgrade.Tier2Slots6 => Tier.Tier2,
                PlayerUpgrade.Tier3Slots3 => Tier.Tier3,
                PlayerUpgrade.Tier3Slots6 => Tier.Tier3,
                _ => null
            };

            if (tier == null) return;

            _inventoryPolicy.AddSlots(tier.Value, 3);
        }
    }
}