using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Player.Retinue.Config;

namespace Features.Player.Retinue.Logic.Modifiers
{
    public sealed class NegotiatorUpgradeCostModifier : BasePercentageModifier
    {
        private readonly CompanionConfig _companionConfig;

        public NegotiatorUpgradeCostModifier(int level) : base(0f, string.Empty)
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            Update(level);
        }

        public void Update(int level)
        {
            var reduction = _companionConfig.NegotiatorData.GetTypedLevelData(level)?.UpgradeCostReduction ?? 0;
            Value.Value = -reduction;
            Description.Value = GetDescription(level);
        }

        private string GetDescription(int level)
        {
            var companionName = _companionConfig.NegotiatorData.Name;
            return $"{companionName} Level {level}";
        }
    }
}