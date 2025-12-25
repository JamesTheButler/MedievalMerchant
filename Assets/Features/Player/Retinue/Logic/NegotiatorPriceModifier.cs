using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Player.Retinue.Config;
using Features.Trade;

namespace Features.Player.Retinue.Logic
{
    public sealed class NegotiatorPriceModifier : BasePercentageModifier
    {
        private readonly TradeType _tradeType;
        private readonly CompanionConfig _companionConfig;

        private int _currentLevel;

        public NegotiatorPriceModifier(int level, TradeType tradeType) : base(0, string.Empty)
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;

            _tradeType = tradeType;
            Update(level);
        }

        public void Update(int level)
        {
            if (_currentLevel == level)
                return;

            var sign = _tradeType == TradeType.Buy ? -1f : 1f;
            var priceBoost = _companionConfig.NegotiatorData.GetTypedLevelData(level)?.PriceSavings ?? 0;
            Value.Value = sign * priceBoost;
            Description.Value = GetDescription(level);
        }

        private string GetDescription(int level)
        {
            return $"{_companionConfig.NegotiatorData.Name} Level {level}";
        }
    }
}