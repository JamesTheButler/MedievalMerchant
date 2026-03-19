using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Trade.Logic.Price
{
    /// <summary>
    /// Static modifier for selling goods to a town that are of the same region as that of the town.
    /// </summary>
    public sealed class LocalGoodPriceModifier : BasePercentageModifier
    {
        public LocalGoodPriceModifier() : base(0,
            ResourceManager.Instance.LocalizationResources.TradeStrings.LocalGoodModifier)
        {
            Value.Value = ConfigurationManager.Configurations.GoodConfig.LocalGoodPriceModifier;
        }
    }
}