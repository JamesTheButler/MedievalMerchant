using Common.Modifiable;
using Features.Inventory;
using Features.Player.Caravan.Logic;
using Features.Player.Retinue.Logic;

namespace Features.Player
{
    public sealed class PlayerModel
    {
        public PlayerLocation Location { get; } = new();

        public ModifiableVariable MovementSpeed => CaravanManager.MoveSpeed;
        public Inventory.Inventory Inventory { get; }
        public RetinueManager RetinueManager { get; }
        public CaravanManager CaravanManager { get; }

        public ModifiableVariable FundsChange { get; }

        public PlayerModel(float startFunds)
        {
            FundsChange = new ModifiableVariable("Funds per day", true);

            RetinueManager = new RetinueManager();
            CaravanManager = new CaravanManager(this);

            var inventoryPolicy = new SlotCountInventoryPolicy(CaravanManager.SlotCount);
            Inventory = new Inventory.Inventory(inventoryPolicy);
            Inventory.AddFunds(startFunds);
        }
    }
}