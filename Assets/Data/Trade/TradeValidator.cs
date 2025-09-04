using Data.Towns;

namespace Data.Trade
{
    public sealed class TradeValidator
    {
        private readonly Player _player;
        private readonly Town _town;

        public TradeValidator(Player player, Town town)
        {
            _player = player;
            _town = town;
        }

        public TradeResult Validate(TradeType tradeType, Good good, int amount)
        {
            if (_town == null)
                return TradeResult.Failed("Cannot complete the trade without a town.");

            if (_town != _player.Location.CurrentTown)
                return TradeResult.Failed($"You are not currently in {_town.Name}");

            var buyingInventory = tradeType == TradeType.Buy ? _player.Inventory : _town.Inventory;
            var sellingInventory = tradeType == TradeType.Sell ? _player.Inventory : _town.Inventory;

            // check if inventory policy prevents the purchase of the good
            var relevantInventoryPolicy = buyingInventory.InventoryPolicy;
            var inventoryPolicyResult = relevantInventoryPolicy.CanAdd(good, amount);
            if (!inventoryPolicyResult.Success)
                return inventoryPolicyResult;

            // check if there are enough items to be sold
            return sellingInventory.HasGood(good, amount)
                ? TradeResult.Succeeded()
                : TradeResult.Failed("Not enough goods to be sold.");
        }
    }
}