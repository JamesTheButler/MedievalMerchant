using Common.Infrastructure.Modifiable;

namespace Features.Combat.Logic
{
    public sealed class CombatStateModifier : BasePercentageModifier
    {
        public CombatStateModifier(float value, string description) : base(value, description) { }
    }
}
