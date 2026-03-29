using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Features.Goods.Config;
using Features.Localization.Data;
using Features.Player.Logic;
using Features.Towns;

namespace Features.Trade.Logic
{
    public sealed class TradeValidator
    {
        private readonly PlayerModel _player;
        private readonly Town _town;
        private readonly GoodResources _goodResources;
        private readonly TradeFailureStrings _loc;

        public TradeValidator(PlayerModel player, Town town)
        {
            _player = player;
            _town = town;
            _goodResources = ResourceManager.Instance.GoodResources;
            _loc = ResourceManager.Instance.LocalizationResources.Trade.FailureStrings;
        }

        public TradeResult Validate(TradeType tradeType, Good good, int amount)
        {
            if (_town == null)
                return TradeResult.Failed(_loc.NoTownSelected());

            if (_town != _player.Location.MapLocation.Value)
                return TradeResult.Failed(_loc.WrongTownSelected(_town.Name));

            var townName = _town.Name;
            var goodName = _goodResources.ResourceData[good].GoodName;
            var buyingInventory = tradeType == TradeType.Buy ? _player.Inventory : _town.Inventory;
            var sellingInventory = tradeType == TradeType.Sell ? _player.Inventory : _town.Inventory;

            if (tradeType == TradeType.Sell && _town.ProductionManager.IsProduced(good))
                return TradeResult.Failed(_loc.GoodProducedInTown(townName, goodName));

            var goodTier = _goodResources.ResourceData[good].Tier;
            if (tradeType == TradeType.Sell && _town.Tier.Value < goodTier)
            {
                return TradeResult.Failed(_loc.InsufficientTier(townName, goodName, goodTier));
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
                    ? _loc.InsufficientGoodTown(townName, goodName)
                    : _loc.InsufficientGoodYou(goodName);
                return TradeResult.Failed(message);
            }

            if (availableAmount < amount)
            {
                var message = tradeType == TradeType.Buy
                    ? _loc.InsufficientAmountTown(townName, goodName)
                    : _loc.InsufficientAmountYou(goodName);
                return TradeResult.Failed(message);
            }

            return TradeResult.Succeeded();
        }
    }
}