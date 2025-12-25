using Common.Infrastructure.Modifiable;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class MilestonePriceBoostModifier : BasePercentageModifier
    {
        public MilestonePriceBoostModifier(float value) : base(value, "Milestone") { }
    }
}