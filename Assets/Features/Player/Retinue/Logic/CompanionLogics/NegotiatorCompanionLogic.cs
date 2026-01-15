using Common.Infrastructure.Gameplay;
using Features.Goods.Selector;
using Features.Player.Retinue.Config.CompanionDatas;
using Features.Player.Retinue.Logic.Modifiers;
using Features.Trade;

namespace Features.Player.Retinue.Logic.CompanionLogics
{
    public sealed class NegotiatorCompanionLogic : BaseCompanionLogic<NegotiatorCompanionData>
    {
        protected override CompanionType Type => CompanionType.Negotiator;

        private readonly NegotiatorUpgradeCostModifier _negotiatorCostModifier;
        private readonly NegotiatorPriceModifier _negotiatorBuyPriceModifier, _negotiatorSellPriceModifier;

        public NegotiatorCompanionLogic()
        {
            _negotiatorBuyPriceModifier = new NegotiatorPriceModifier(0, TradeType.Buy);
            _negotiatorSellPriceModifier = new NegotiatorPriceModifier(0, TradeType.Sell);
            _negotiatorCostModifier = new NegotiatorUpgradeCostModifier(0);
            
            var gameModel = GameplayContext.Instance.Model;
            var caravanManager = gameModel.Player.CaravanManager;
            
            foreach (var cart in caravanManager.Carts)
            {
                cart.UpgradeCost.AddModifier(_negotiatorCostModifier);
            }

            foreach (var town in gameModel.Towns.Values)
            {
                town.PriceManager.AddModifier(
                    _negotiatorBuyPriceModifier,
                    IGoodSelector.All,
                    TradeType.Buy);

                town.PriceManager.AddModifier(
                    _negotiatorSellPriceModifier,
                    IGoodSelector.All,
                    TradeType.Sell);
            }
        }

        public override void SetLevel(int level)
        {
            if (level <= 0)
                return;

            _negotiatorCostModifier.Update(level);
            _negotiatorBuyPriceModifier.Update(level);
            _negotiatorSellPriceModifier.Update(level);
        }
    }
}