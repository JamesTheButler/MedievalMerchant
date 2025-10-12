using Common;
using Common.Modifiable;

namespace Features.Player.Retinue.Logic
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