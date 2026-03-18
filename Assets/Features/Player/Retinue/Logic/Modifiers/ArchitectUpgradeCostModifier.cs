using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Localization.Data;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Config.Resources;

namespace Features.Player.Retinue.Logic.Modifiers
{
    public sealed class ArchitectUpgradeCostModifier : BasePercentageModifier
    {
        private int _currentLevel = -1;

        private readonly CompanionConfig _companionConfig;
        private readonly CompanionResource _companionResource;
        private readonly CompanionLocalizationResources _loc;

        public ArchitectUpgradeCostModifier(int level) : base(0, string.Empty)
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _loc = ResourceManager.Instance.LocalizationResources.Player.Companions;
            _companionResource = ResourceManager.Instance.CompanionResources.Architect;

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
            return _loc.CompanionDisplayString(_companionResource.Name, level);
        }
    }
}