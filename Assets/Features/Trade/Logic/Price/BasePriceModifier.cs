using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;

namespace Features.Trade.Logic.Price
{
    public sealed class BasePriceModifier : BaseValueModifier
    {
        public BasePriceModifier(float value, Tier goodTier) : base(value, GetDescription(goodTier)) { }

        private static string GetDescription(Tier goodTier)
        {
            return ResourceManager.Instance.LocalizationResources.TradeStrings.BasePrice(goodTier);
        }
    }
}