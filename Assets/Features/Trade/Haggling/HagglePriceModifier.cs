using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Trade.Haggling.Data;

namespace Features.Trade.Haggling
{
    public sealed class HagglePriceModifier : BasePercentageModifier
    {
        private readonly TradeType _tradeType;

        private readonly HaggleConfig _haggleConfig;
        private readonly HaggleResources _haggleResources;

        public HagglePriceModifier(HaggleLevel haggleLevel, TradeType tradeType) : base(0f, string.Empty)
        {
            _tradeType = tradeType;
            _haggleConfig = ConfigurationManager.Configurations.HaggleConfig;
            _haggleResources = ResourceManager.Instance.HaggleResources;

            Update(haggleLevel);
        }

        public void Update(HaggleLevel haggleLevel)
        {
            var sign = _tradeType == TradeType.Buy ? 1 : -1;
            Value.Value = sign * _haggleConfig.Configs[haggleLevel].PriceDifferenceOnBuy;
            Description.Value = $"You are haggling {_haggleResources.HaggleLevelNames[haggleLevel]}ly.";
        }
    }
}