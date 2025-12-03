using Common;
using Common.Modifiable;
using Features.Player.Retinue.Config;

namespace Features.Player.Retinue.Logic
{
    public sealed class NavigatorSpeedModifier : BasePercentageModifier
    {
        private readonly CompanionConfig _companionConfig;

        public NavigatorSpeedModifier(int level) : base(0f, string.Empty)
        {
            _companionConfig = ConfigurationManager.Configurations.CompanionConfig;

            Update(level);
        }

        public void Update(int level)
        {
            Value.Value = _companionConfig.NavigatorData.GetTypedLevelData(level).SpeedBonus;
            Description.Value = GetDescription(level);
        }

        private static string GetDescription(int level)
        {
            return $"Navigator level {level}";
        }
    }
}