using Features.Levels.Conditions.Logic;

namespace Features.Levels.Conditions.Model
{
    public interface ICondition
    {
        ConditionType Type { get; }
        Progress Progress { get; }
        string Description { get; }
    }
}