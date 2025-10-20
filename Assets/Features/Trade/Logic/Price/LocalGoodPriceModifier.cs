using Common;
using Common.Modifiable;

namespace Features.Trade.Logic.Price
{
    /// <summary>
    /// Static modifier for selling goods to a town that are of the same region as that of the town.
    /// </summary>
    public sealed class LocalGoodPriceModifier : BasePercentageModifier
    {
        public LocalGoodPriceModifier() : base(0, "Good from local region.")
        {
            Value.Value = ConfigurationManager.Instance.GoodsConfig.LocalGoodPriceModifier;
        }
    }
}