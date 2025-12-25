using Common.Infrastructure.Modifiable;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class MilestoneFundsBoostModifier : BasePercentageModifier
    {
        public MilestoneFundsBoostModifier(float value) : base(value, "Milestone") { }
    }
}