using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Player.Retinue.Config;

namespace Features.Player.Retinue.Logic.Modifiers
{
    public sealed class ArchitectUpgradeCostModifier : BasePercentageModifier
    {
        private int _currentLevel = -1;
        private readonly CompanionConfig _companionConfig;

        public ArchitectUpgradeCostModifier(int level) : base(0, string.Empty)
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            Update(level);
        }

        public void Update(int level)
        {
            if (_currentLevel == level)
                return;

            _currentLevel = level;
            var priceBoost = _companionConfig.ArchitectData.GetTypedLevelData(level)?.ConstructionPriceReduction ?? 0;
            Value.Value = -priceBoost;
            Description.Value = GetDescription(level);
        }

        private string GetDescription(int level)
        {
            return $"{_companionConfig.ArchitectData.Name} Level {level}";
        }
    }
}