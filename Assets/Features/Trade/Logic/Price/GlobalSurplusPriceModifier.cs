using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Features.Localization.Data;
using UnityEngine;

namespace Features.Trade.Logic.Price
{
    public sealed class GlobalSurplusPriceModifier : BasePercentageModifier
    {
        private readonly string _goodName;
        private readonly GlobalSurplusModiferConfigData _config;
        private readonly TradeLocalizationResources _loc;

        private int _previousStep;

        public GlobalSurplusPriceModifier(Good good, int value) : base(0f, string.Empty)
        {
            _loc = ResourceManager.Instance.LocalizationResources.Trade;
            _config = ConfigurationManager.Configurations.PriceModifierConfig.GlobalSurplusModiferConfig;
            var goodRes = ResourceManager.Instance.GoodResources;
            _goodName = goodRes.ResourceData[good].GoodName;

            Update(value);
        }

        public void Update(int amount)
        {
            Description.Value = _loc.GlobalSurplusDescription(
                amount,
                _goodName,
                _config.PriceReductionPerStep,
                _config.GoodsPerStep);

            // amount adjusted to start threshold
            var adjustedAmount = amount - _config.StartThreshold;
            var currentStep = Mathf.FloorToInt((float)adjustedAmount / _config.GoodsPerStep);

            if (_previousStep == currentStep)
                return;

            _previousStep = currentStep;
            var newValue = _config.PriceReductionPerStep * currentStep;
            Value.Value = newValue;
        }
    }
}