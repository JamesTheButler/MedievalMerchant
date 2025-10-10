using Data.Modifiable;
using Data.Player.Caravan.Logic;
using Data.Player.Retinue;
using Data.Trade;

namespace Data.Player
{
    public sealed class PlayerModel
    {
        public PlayerLocation Location { get; } = new();

        public ModifiableVariable MovementSpeed => CaravanManager.MoveSpeed;
        public Inventory Inventory { get; }
        public RetinueManager RetinueManager { get; }
        public CaravanManager CaravanManager { get; }

        private readonly IInventoryPolicy _inventoryPolicy;

        public PlayerModel(float startFunds)
        {
            RetinueManager = new RetinueManager();
            CaravanManager = new CaravanManager();

            _inventoryPolicy = new SlotCountInventoryPolicy(CaravanManager.SlotCount);
            Inventory = new Inventory(_inventoryPolicy);
            Inventory.AddFunds(startFunds);
        }
    }
}