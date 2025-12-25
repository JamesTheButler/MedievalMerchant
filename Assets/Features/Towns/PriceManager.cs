using System.Collections.Generic;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Features.Goods.Selector;
using Features.Trade;
using Features.Trade.Logic.Price;

namespace Features.Towns
{
    public sealed class PriceManager
    {
        private readonly Town _town;

        private readonly ReputationPriceModifier _reputationBuyModifier, _reputationSellModifier;
        // town milestones

        private readonly List<IModifier> _milestoneModifiers = new();
        private readonly PriceList _buyPrices, _sellPrices;

        public PriceManager(Town town)
        {
            _town = town;
            var productionManager = _town.ProductionManager;
            _buyPrices = new PriceList(TradeType.Buy, town, productionManager.IsProduced);
            _sellPrices = new PriceList(TradeType.Sell, town, good => !productionManager.IsProduced(good));

            _reputationBuyModifier = new ReputationPriceModifier(town, TradeType.Buy);
            _buyPrices.AddModifier(_reputationBuyModifier, new AllGoodsSelector());

            _reputationSellModifier = new ReputationPriceModifier(town, TradeType.Sell);
            _sellPrices.AddModifier(_reputationSellModifier, new AllGoodsSelector());

            _sellPrices.AddModifier(
                new LocalGoodPriceModifier(),
                new ComplexGoodSelector(regions: town.Regions));
            _sellPrices.AddModifier(
                new ForeignGoodPriceModifier(),
                new ComplexGoodSelector(regions: Regions.All & ~town.Regions));
        }

        public ModifiableVariable GetPrice(Good good, TradeType tradeType)
        {
            return tradeType == TradeType.Buy ? _buyPrices.GetPrice(good) : _sellPrices.GetPrice(good);
        }

        public void AddModifier(
            IModifier modifier,
            IGoodSelector goodSelector,
            TradeType tradeType)
        {
            var priceList = tradeType == TradeType.Buy ? _buyPrices : _sellPrices;
            priceList.AddModifier(modifier, goodSelector);
        }

        public void RemoveModifier(
            IModifier modifier,
            TradeType tradeType)
        {
            var priceList = tradeType == TradeType.Buy ? _buyPrices : _sellPrices;
            priceList.RemoveModifier(modifier);
        }
    }
}