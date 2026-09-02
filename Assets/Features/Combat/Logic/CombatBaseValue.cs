using Common.Infrastructure.Modifiable;

namespace Features.Combat.Logic
{
    public sealed class CombatBaseValue : BaseValueModifier
    {
        public CombatBaseValue(float value, string description) : base(value, description) { }
    }
}
