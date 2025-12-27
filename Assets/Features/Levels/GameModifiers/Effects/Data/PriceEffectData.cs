using Common.Types;
using Common.UI.Utility;
using Common.Utility;
using Features.Goods.Selector;
using Features.Trade;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [CreateAssetMenu(
        fileName = nameof(PriceEffectData),
        menuName = AssetMenu.EffectsFolder + nameof(PriceEffectData))]
    public sealed class PriceEffectData : EffectData
    {
        [field: SerializeField]
        public TradeTypes TradeTypes { get; private set; } = TradeTypes.All;

        [field: SerializeField, Range(-1f, 2f)]
        public float PriceBoostPercent { get; private set; }

        [field: SerializeField]
        public GoodSelectorData GoodSelector { get; private set; }

        private string _description;

        public override string Description
        {
            get
            {
                var valueString = PriceBoostPercent.ToPercentString(true);
                var tradeTypeString = TradeTypes switch
                {
                    TradeTypes.All or (TradeTypes)(-1) => "",
                    TradeTypes.Buy => "purchase ",
                    TradeTypes.Sell => "sale ",
                };
                var selectorString = GoodSelector.Selector.ToDisplayString();
                var style = PriceBoostPercent > 0 ? Style.Good : Style.Bad;
                return $"{valueString} to {tradeTypeString}prices {selectorString}".WithStyle(style);
            }
        }
    }
}