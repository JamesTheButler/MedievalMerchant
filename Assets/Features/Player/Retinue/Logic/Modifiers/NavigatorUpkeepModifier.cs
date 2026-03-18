using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Localization.Data;
using Features.Player.Retinue.Config;
using Features.Player.Retinue.Config.Resources;

namespace Features.Player.Retinue.Logic.Modifiers
{
    public sealed class NavigatorUpkeepModifier : BasePercentageModifier
    {
        private readonly CompanionConfig _companionConfig;
        private readonly CompanionResource _companionResource;
        private readonly CompanionLocalizationResources _loc;

        public NavigatorUpkeepModifier(int level) : base(0f, string.Empty)
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;
            _loc = ResourceManager.Instance.LocalizationResources.Player.Companions;
            _companionResource = ResourceManager.Instance.CompanionResources.Navigator;

            Update(level);
        }

        public void Update(int level)
        {
            Value.Value = -_companionConfig.NavigatorData.GetTypedLevelData(level)?.UpkeepReduction ?? 0;
            Description.Value = GetDescription(level);
        }

        private string GetDescription(int level)
        {
            return _loc.CompanionDisplayString(_companionResource.Name, level);
        }
    }
}