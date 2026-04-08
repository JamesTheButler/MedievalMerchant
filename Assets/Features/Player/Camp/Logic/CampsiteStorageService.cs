using System;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Goods.Config;
using Features.Localization.Data;
using Features.Trade;

namespace Features.Player.Camp.Logic
{
    public sealed class CampsiteStorageService : IService
    {
        private Inventory.Inventory _camp, _player;
        private TradeFailureStrings _loc;

        public void Initialize()
        {
            _camp = GameplayContext.Instance.Model.Camp.Inventory;
            _player = GameplayContext.Instance.Model.Player.Inventory;
            _loc = ResourceManager.Instance.LocalizationResources.Trade.FailureStrings;
        }

        public void CleanUp() { }

        private TradeResult Transfer(
            Inventory.Inventory from,
            Inventory.Inventory to,
            Good good,
            int amount,
            Func<Good, string> insufficientGoodLocalizer)
        {
            if (!from.HasGood(good, amount))
                return TradeResult.Failed(insufficientGoodLocalizer.Invoke(good));

            var addResult = to.InventoryPolicy.CanAdd(good, amount);
            if (addResult.HasError)
                return addResult;

            from.RemoveGood(good, amount);
            to.AddGood(good, amount);

            return TradeResult.Succeeded();
        }

        public TradeResult TransferToCamp(Good good, int amount)
        {
            return Transfer(_player, _camp, good, amount, _loc.InsufficientGoodYou);
        }

        public TradeResult TransferToPlayer(Good good, int amount)
        {
            return Transfer(_camp, _player, good, amount, _loc.InsufficientGoodsCamp);
        }
    }
}