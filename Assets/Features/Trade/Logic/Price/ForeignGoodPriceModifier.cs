using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Trade.Logic.Price
{
    /// <summary>
    /// Static modifier for selling goods to a town that are of a region that the town is not in.
    /// </summary>
    public sealed class ForeignGoodPriceModifier : BasePercentageModifier
    {
        public ForeignGoodPriceModifier() : base(0, "Good from foreign region")
        {
            Value.Value = ConfigurationManager.Configurations.GoodConfig.ForeignGoodPriceModifier;
        }
    }
}