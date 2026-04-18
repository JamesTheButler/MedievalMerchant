using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Features.Inventory;
using Features.Player.Caravan.Logic;
using Features.Player.Retinue.Logic;
using JetBrains.Annotations;

namespace Features.Player.Logic
{
    public sealed class CaravanSlotInventorySystem : ISystem
    {
        private Inventory.Inventory _inventory;
        private CaravanManager _caravan;

        public void Initialize() { }
        public void CleanUp() { }
    }

    public sealed class PlayerModel
    {
        public readonly Observable<float> SpeedInTilesPerDay = new(1f);

        public PlayerLocation Location { get; } = new();
        public ModifiableVariable MovementSpeed => CaravanManager.MoveSpeed;

        public ModifiableVariable FundsChange { get; }
        public Inventory.Inventory Inventory { get; }
        public RetinueModel RetinueModel { get; }
        public CaravanManager CaravanManager { get; }
        public TradeTracker TradeTracker { get; }

        private readonly SlotCountInventoryPolicy _inventoryPolicy;

        public PlayerModel(float startFunds)
        {
            var loc = ResourceManager.Instance.LocalizationResources.Player;
            FundsChange = new ModifiableVariable(loc.FundsChangeModifier, true);

            RetinueModel = new RetinueModel();
            CaravanManager = new CaravanManager();
            TradeTracker = new TradeTracker();

            _inventoryPolicy = new SlotCountInventoryPolicy(CaravanManager.SlotCount);
            CaravanManager.SlotCount.Observe(RefreshInventoryPolicy);

            Inventory = new Inventory.Inventory(_inventoryPolicy);
            Inventory.AddFunds(startFunds);

            CaravanManager.SlotCount.Observe(RefreshInventoryPolicy);
        }

        private void RefreshInventoryPolicy(int slotCount)
        {
            _inventoryPolicy.SetSlotCount(slotCount);
        }
    }
}