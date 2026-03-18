using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using Features.Inventory;
using Features.Player.Caravan.Logic;
using Features.Player.Retinue.Logic;

namespace Features.Player.Logic
{
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

        public PlayerModel(float startFunds)
        {
            var loc = ResourceManager.Instance.LocalizationResources.Player;
            FundsChange = new ModifiableVariable(loc.FundsChangeModifier.GetLocalizedString(), true);

            RetinueModel = new RetinueModel();
            CaravanManager = new CaravanManager();
            TradeTracker = new TradeTracker();

            var inventoryPolicy = new SlotCountInventoryPolicy(CaravanManager.SlotCount);
            Inventory = new Inventory.Inventory(inventoryPolicy);
            Inventory.AddFunds(startFunds);
        }
    }
}