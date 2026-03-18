using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Localization.Data;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Config.Resources;
using Features.Trade;

namespace Features.Player.Retinue.Logic.Modifiers
{
    public sealed class NegotiatorPriceModifier : BasePercentageModifier
    {
        private readonly TradeType _tradeType;
        private readonly CompanionConfig _companionConfig;
        private readonly CompanionResource _companionResource;
        private readonly CompanionLocalizationResources _loc;

        private int _currentLevel = -1;

        public NegotiatorPriceModifier(int level, TradeType tradeType) : base(0, string.Empty)
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _loc = ResourceManager.Instance.LocalizationResources.Player.Companions;
            _companionResource = ResourceManager.Instance.CompanionResources.Navigator;

            _tradeType = tradeType;
            Update(level);
        }

        public void Update(int level)
        {
            if (_currentLevel == level)
                return;

            _currentLevel = level;
            var sign = _tradeType == TradeType.Buy ? -1f : 1f;
            var priceBoost = _companionConfig.NegotiatorData.GetTypedLevelData(level)?.PriceSavings ?? 0;
            Value.Value = sign * priceBoost;
            Description.Value = GetDescription(level);
        }

        private string GetDescription(int level)
        {
            return _loc.CompanionDisplayString(_companionResource.Name, level);
        }
    }
}