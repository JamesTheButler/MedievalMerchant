using Common;
using Common.Modifiable;
using Features.Player.Retinue.Config;
using Features.Trade;
using UnityEngine.PlayerLoop;

namespace Features.Player.Retinue.Logic
{
    public sealed class NegotiatorPriceModifier : BasePercentageModifier
    {
        private readonly TradeType _tradeType;
        private readonly CompanionConfig _companionConfig = ConfigurationManager.Instance.CompanionConfig;

        private int _currentLevel;

        public NegotiatorPriceModifier(int level, TradeType tradeType) : base(0, string.Empty)
        {
            _tradeType = tradeType;
            Update(level);
        }

        public void Update(int level)
        {
            if (_currentLevel == level)
                return;

            var sign = _tradeType == TradeType.Buy ? -1f : 1f;
            var priceBoost = _companionConfig.NegotiatorData.GetTypedLevelData(level).PriceSavings;
            Value.Value = sign * priceBoost;
            Description.Value = GetDescription(level);
        }

        private string GetDescription(int level)
        {
            return $"{_companionConfig.NegotiatorData.Name} Level {level}";
        }
    }
}