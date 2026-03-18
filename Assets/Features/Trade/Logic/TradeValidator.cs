using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Features.Goods.Config;
using Features.Player.Logic;
using Features.Towns;

namespace Features.Trade.Logic
{
    public sealed class TradeValidator
    {
        private readonly PlayerModel _player;
        private readonly Town _town;
        private readonly GoodResources _goodResources;

        public TradeValidator(PlayerModel player, Town town)
        {
            _player = player;
            _town = town;
            _goodResources = ResourceManager.Instance.GoodResources;
        }

        public TradeResult Validate(TradeType tradeType, Good good, int amount)
        {
            var townName = _town.Name;
            var goodName = _goodResources.ResourceData[good].GoodName;

            if (_town == null)
                return TradeResult.Failed("Travel to and select a town to trade.");

            if (_town != _player.Location.CurrentTown.Value)
                return TradeResult.Failed($"Travel to {townName} to trade with them.");

            var buyingInventory = tradeType == TradeType.Buy ? _player.Inventory : _town.Inventory;
            var sellingInventory = tradeType == TradeType.Sell ? _player.Inventory : _town.Inventory;

            if (tradeType == TradeType.Sell && _town.ProductionManager.IsProduced(good))
                return TradeResult.Failed(
                    $"{townName} is producing {goodName} themselves. They aren't interested in buying it.");

            var goodTier = _goodResources.ResourceData[good].Tier;
            if (tradeType == TradeType.Sell && _town.Tier.Value < goodTier)
            {
                return TradeResult.Failed(
                    $"{townName} cannot buy {goodName} as they are not {goodTier.ToDisplayString()} yet.");
            }

            // check if inventory policy prevents the purchase of the good
            var relevantInventoryPolicy = buyingInventory.InventoryPolicy;
            var inventoryPolicyResult = relevantInventoryPolicy.CanAdd(good, amount);
            if (!inventoryPolicyResult.Success)
                return inventoryPolicyResult;

            var availableAmount = sellingInventory.Goods.GetValueOrDefault(good, 0);
            if (availableAmount == 0)
            {
                var message = tradeType == TradeType.Buy
                    ? $"{townName} does not own any {goodName}."
                    : $"You do not own any {goodName}.";
                return TradeResult.Failed(message);
            }

            if (availableAmount < amount)
            {
                var message = tradeType == TradeType.Buy
                    ? $"{townName} does not own enough {goodName}."
                    : $"You do not own enough {goodName}.";
                return TradeResult.Failed(message);
            }

            return TradeResult.Succeeded();
        }
    }
}