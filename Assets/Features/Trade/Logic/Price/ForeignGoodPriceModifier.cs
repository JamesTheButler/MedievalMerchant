using Common;
using Common.Modifiable;

namespace Features.Trade.Logic.Price
{
    /// <summary>
    /// Static modifier for selling goods to a town that are of a region that the town is not in.
    /// </summary>
    public sealed class ForeignGoodPriceModifier : BasePercentageModifier
    {
        public ForeignGoodPriceModifier() : base(0, "Foreign good price modifier")
        {
            Value.Value = ConfigurationManager.Instance.GoodsConfig.ForeignGoodPriceModifier;
        }
    }
}