using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Player.Retinue.Logic.Modifiers
{
    public sealed class CompanionUpgradeBaseCostModifier : BaseValueModifier
    {
        public CompanionUpgradeBaseCostModifier(float value) : base(value, GetDescription()) { }

        private static string GetDescription()
        {
            var loc = ResourceManager.Instance.LocalizationResources;
            return loc.Player.UpgradeCost;
        }
    }
}