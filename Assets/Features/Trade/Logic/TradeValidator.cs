using Common.Types;
using Features.Player.Logic;
using Features.Towns;

namespace Features.Trade.Logic
{
    public sealed class TradeValidator
    {
        private readonly PlayerModel _player;
        private readonly Town _town;

        public TradeValidator(PlayerModel player, Town town)
        {
            _player = player;
            _town = town;
        }

        public TradeResult Validate(TradeType tradeType, Good good, int amount)
        {
            if (_town == null)
                return TradeResult.Failed("Travel to and select a town to trade.");

            if (_town != _player.Location.CurrentTown.Value)
                return TradeResult.Failed($"Travel to {_town.Name} to trade with them.");

            var buyingInventory = tradeType == TradeType.Buy ? _player.Inventory : _town.Inventory;
            var sellingInventory = tradeType == TradeType.Sell ? _player.Inventory : _town.Inventory;

            if (tradeType == TradeType.Sell && _town.ProductionManager.IsProduced(good))
                return TradeResult.Failed(
                    $"This good is produced in {_town.Name}. They aren't interested in buying it.");

            // check if inventory policy prevents the purchase of the good
            var relevantInventoryPolicy = buyingInventory.InventoryPolicy;
            var inventoryPolicyResult = relevantInventoryPolicy.CanAdd(good, amount);
            if (!inventoryPolicyResult.Success)
                return inventoryPolicyResult;

            var notEnoughGoodsMessage = tradeType == TradeType.Buy
                ? $"{_town.Name} does not have that many goods in their inventory."
                : "You do not have that many goods in your inventory.";

            // check if there are enough items to be sold
            return sellingInventory.HasGood(good, amount)
                ? TradeResult.Succeeded()
                : TradeResult.Failed(notEnoughGoodsMessage);
        }
    }
}