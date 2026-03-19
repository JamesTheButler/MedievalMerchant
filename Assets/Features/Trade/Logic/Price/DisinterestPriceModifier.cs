using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Features.Localization.Data;
using UnityEngine;

namespace Features.Trade.Logic.Price
{
    public sealed class DisinterestPriceModifier : BasePercentageModifier
    {
        private readonly string _goodName;
        private readonly DisinterestModiferConfigData _config;
        private readonly TradeLocalizationResources _loc;

        private int _previousStep;

        public DisinterestPriceModifier(Good good, int value) : base(0f, string.Empty)
        {
            _loc = ResourceManager.Instance.LocalizationResources.TradeStrings;
            _config = ConfigurationManager.Configurations.PriceModifierConfig.DisinterestModiferConfig;

            var goodRes = ResourceManager.Instance.GoodResources;
            _goodName = goodRes.ResourceData[good].GoodName;

            Update(value);
        }

        public void Update(int amount)
        {
            Description.Value = _loc.DisinterestDescription(
                _goodName,
                amount,
                _config.TrackedPeriodInDays,
                _config.PriceReductionPerStep,
                _config.GoodsPerStep);

            var currentStep = Mathf.FloorToInt((float)amount / _config.GoodsPerStep);

            if (_previousStep == currentStep)
                return;

            _previousStep = currentStep;
            var newValue = _config.PriceReductionPerStep * currentStep;
            Value.Value = newValue;
        }
    }
}