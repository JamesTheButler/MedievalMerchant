using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Player.Retinue.Config;

namespace Features.Player.Retinue.Logic
{
    public sealed class NavigatorUpkeepModifier : BasePercentageModifier
    {
        private readonly CompanionConfig _companionConfig;

        public NavigatorUpkeepModifier(int level) : base(0f, string.Empty)
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;

            Update(level);
        }

        public void Update(int level)
        {
            Value.Value = -_companionConfig.NavigatorData.GetTypedLevelData(level)?.UpkeepReduction ?? 0;
            Description.Value = GetDescription(level);
        }

        private static string GetDescription(int level)
        {
            return $"Navigator level {level}";
        }
    }
}