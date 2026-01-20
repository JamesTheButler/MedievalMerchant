using System.Text;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Trade.Logic.Price
{
    public sealed class DisinterestPriceModifier : BasePercentageModifier
    {
        private readonly string _goodName;
        private readonly DisinterestModiferConfigData _config;

        private int _previousStep;

        public DisinterestPriceModifier(Good good, int value) : base(0f, string.Empty)
        {
            _config = ConfigurationManager.Configurations.PriceModifierConfig.DisinterestModiferConfig;

            var goodRes = ResourceManager.Instance.GoodResources;
            _goodName = goodRes.ResourceData[good].GoodName;

            Update(value);
        }

        public void Update(int amount)
        {
            var descriptionStringBuilder = new StringBuilder()
                .AppendLine($"This town is growing tired of {_goodName}.")
                .AppendLine($"They bought {amount} {_goodName} in the last {_config.TrackedPeriodInDays} days.")
                .AppendLine($"({_config.PriceReductionPerStep.ToPercentString()} per {_config.GoodsPerStep} goods)");

            Description.Value = descriptionStringBuilder.ToString();

            var currentStep = Mathf.FloorToInt((float)amount / _config.GoodsPerStep);

            if (_previousStep == currentStep)
                return;

            _previousStep = currentStep;
            var newValue = _config.PriceReductionPerStep * currentStep;
            Value.Value = newValue;
        }
    }
}