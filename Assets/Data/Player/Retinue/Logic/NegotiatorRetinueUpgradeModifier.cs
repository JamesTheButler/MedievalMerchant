using Data.Configuration;
using Data.Modifiable;

namespace Data.Player.Retinue.Logic
{
    public sealed class NegotiatorRetinueUpgradeModifier : BasePercentageModifier
    {
        public NegotiatorRetinueUpgradeModifier(float value, int level) : base(value, GetDescription(level))
        {
        }

        private static string GetDescription(int level)
        {
            var companionName = ConfigurationManager.Instance.CompanionConfig.NegotiatorData.Name;
            return $"{companionName} Level {level}";
        }
    }
}