using Common.Infrastructure.Modifiable;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionUpgradeBaseCostModifier : BaseValueModifier
    {
        public CompanionUpgradeBaseCostModifier(float value) : base(value, "Upgrade Cost") { }
    }
}