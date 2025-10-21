using Common.Modifiable;
using Features.Inventory;
using Features.Player.Caravan.Logic;
using Features.Player.Retinue;
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

        private readonly IInventoryPolicy _inventoryPolicy;

        public PlayerModel(float startFunds)
        {
            RetinueManager = new RetinueManager();
            CaravanManager = new CaravanManager();

            _inventoryPolicy = new SlotCountInventoryPolicy(CaravanManager.SlotCount);
            Inventory = new Inventory.Inventory(_inventoryPolicy);
            Inventory.AddFunds(startFunds);
        }
    }
}