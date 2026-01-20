using System.Text;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Trade.Logic.Price
{
    public sealed class GlobalSurplusPriceModifier : BasePercentageModifier
    {
        private readonly string _goodName;
        private readonly GlobalSurplusModiferConfigData _config;

        private int _previousStep;

        public GlobalSurplusPriceModifier(Good good, int value) : base(0f, string.Empty)
        {
            _config = ConfigurationManager.Configurations.PriceModifierConfig.GlobalSurplusModiferConfig;
            var goodRes = ResourceManager.Instance.GoodResources;
            _goodName = goodRes.ResourceData[good].GoodName;

            Update(value);
        }

        public void Update(int amount)
        {
            var descriptionStringBuilder = new StringBuilder()
                .AppendLine($"There is a global surplus of {amount} {_goodName}.")
                .AppendLine($"({_config.PriceReductionPerStep.ToPercentString()} per {_config.GoodsPerStep} goods)");

            Description.Value = descriptionStringBuilder.ToString();

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