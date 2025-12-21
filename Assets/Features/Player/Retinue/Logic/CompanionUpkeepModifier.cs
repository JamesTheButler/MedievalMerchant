using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Features.Player.Retinue.Config;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionUpkeepModifier : FlatModifier
    {
        private readonly CompanionConfigData _configData;

        public CompanionUpkeepModifier(CompanionType companionType) : base(0f, string.Empty)
        {
            _configData = ConfigurationManager.Configurations.CompanionConfig.Get(companionType);
            SetLevel(0);
        }

        public void SetLevel(int level)
        {
            var levelData = _configData.GetLevelData(level);
            Value.Value = levelData?.Upkeep ?? 0f;
            Description.Value = _configData.DisplayString(level);
        }
    }
}