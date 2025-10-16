using Common.Modifiable;

namespace Features.Player.Retinue
{
    public sealed class CompanionUpgradeBaseCostModifier : BaseValueModifier
    {
        public CompanionUpgradeBaseCostModifier(float value) : base(value, "Upgrade Cost") { }
    }
}