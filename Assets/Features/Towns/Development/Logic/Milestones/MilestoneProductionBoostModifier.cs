using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Towns.Development.Logic.Milestones
{
    public sealed class MilestoneProductionBoostModifier : BasePercentageModifier
    {
        public MilestoneProductionBoostModifier(float value) :
            base(value, ResourceManager.Instance.LocalizationResources.Town.Milestones.Title) { }
    }
}