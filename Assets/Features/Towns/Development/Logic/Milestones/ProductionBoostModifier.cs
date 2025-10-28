using Common.Modifiable;

namespace Features.Towns.Development.Logic.Milestones
{
    public abstract class ProductionBoostModifier : BasePercentageModifier
    {
        protected ProductionBoostModifier(float value, string description) : base(value, description) { }
    }
}