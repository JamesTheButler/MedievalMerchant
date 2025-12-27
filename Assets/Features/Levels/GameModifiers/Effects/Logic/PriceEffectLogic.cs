using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Utility;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Levels.GameModifiers.Logic;
using Features.Trade;

namespace Features.Levels.GameModifiers.Effects.Logic
{
    public sealed class PriceEffectLogic : EffectLogic<PriceEffectData>
    {
        private readonly GameplayModel _gameplayModel;

        private IModifier _buyPriceModifier, _sellPriceModifier;

        public PriceEffectLogic(EffectOrigin effectOrigin, PriceEffectData effectData)
            : base(effectOrigin, effectData)
        {
            _gameplayModel = GameplayContext.Instance.Model;
        }

        public override void Apply()
        {
            _buyPriceModifier = new EffectPercentModifier(-EffectData.PriceBoostPercent, EffectOrigin);
            _sellPriceModifier = new EffectPercentModifier(EffectData.PriceBoostPercent, EffectOrigin);
            var goodSelector = EffectData.GoodSelector.Selector;
            foreach (var town in _gameplayModel.Towns.Values)
            {
                if (EffectData.TradeTypes.Intersects(TradeTypes.Buy))
                {
                    town.PriceManager.AddModifier(_buyPriceModifier, goodSelector, TradeType.Buy);
                }

                if (EffectData.TradeTypes.Intersects(TradeTypes.Sell))
                {
                    town.PriceManager.AddModifier(_sellPriceModifier, goodSelector, TradeType.Sell);
                }
            }
        }

        public override void Unapply()
        {
            foreach (var town in _gameplayModel.Towns.Values)
            {
                if (EffectData.TradeTypes.Intersects(TradeTypes.Buy) && _buyPriceModifier != null)
                {
                    town.PriceManager.RemoveModifier(_buyPriceModifier, TradeType.Buy);
                }

                if (EffectData.TradeTypes.Intersects(TradeTypes.Sell) && _sellPriceModifier != null)
                {
                    town.PriceManager.RemoveModifier(_sellPriceModifier, TradeType.Sell);
                }
            }

            _buyPriceModifier = null;
            _sellPriceModifier = null;
        }
    }
}