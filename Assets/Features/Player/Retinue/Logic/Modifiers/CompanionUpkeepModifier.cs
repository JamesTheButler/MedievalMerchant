using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Localization.Data;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Config.CompanionDatas;
using Features.Player.Retinue.Config.Resources;

namespace Features.Player.Retinue.Logic.Modifiers
{
    public sealed class CompanionUpkeepModifier : FlatModifier
    {
        private readonly CompanionConfigData _configData;
        private readonly CompanionResource _companionResource;
        private readonly CompanionLocalizationResources _loc;

        public CompanionUpkeepModifier(CompanionType companionType) : base(0f, string.Empty)
        {
            _configData = ConfigurationManager.Configurations.CompanionConfig.Get(companionType);
            _loc = ResourceManager.Instance.LocalizationResources.Player.Companions;
            _companionResource = ResourceManager.Instance.CompanionResources.Get(companionType);
            SetLevel(0);
        }

        public void SetLevel(int level)
        {
            var levelData = _configData.GetLevelData(level);
            Value.Value = levelData?.Upkeep ?? 0f;
            Description.Value = _loc.CompanionDisplayString(_companionResource.Name, level);
        }
    }
}