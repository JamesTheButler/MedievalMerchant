using Common.Infrastructure.Modifiable;
using Common.Types;
using Features.Goods.Selector;
using Features.Trade;
using Features.Trade.Logic.Price;

namespace Features.Towns
{
    public sealed class PriceManager
    {
        private readonly PriceList _buyPrices, _sellPrices;

        public PriceManager(Town town)
        {
            var productionManager = town.ProductionManager;
            _buyPrices = new PriceList(TradeType.Buy, town, productionManager.IsProduced);
            _sellPrices = new PriceList(TradeType.Sell, town, good => !productionManager.IsProduced(good));

            var reputationBuyModifier = new ReputationPriceModifier(town, TradeType.Buy);
            _buyPrices.AddModifier(reputationBuyModifier, IGoodSelector.All);

            var reputationSellModifier = new ReputationPriceModifier(town, TradeType.Sell);
            _sellPrices.AddModifier(reputationSellModifier, IGoodSelector.All);

            _sellPrices.AddModifier(
                new LocalGoodPriceModifier(),
                new ComplexGoodSelector(selectedRegions: town.Regions));
            _sellPrices.AddModifier(
                new ForeignGoodPriceModifier(),
                new ComplexGoodSelector(selectedRegions: Regions.All & ~town.Regions));
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