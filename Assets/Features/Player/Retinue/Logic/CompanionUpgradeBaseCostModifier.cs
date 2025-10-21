using Common.Modifiable;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionUpgradeBaseCostModifier : BaseValueModifier
    {
        public CompanionUpgradeBaseCostModifier(float value) : base(value, "Upgrade Cost") { }
    }
}