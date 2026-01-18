using System;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public abstract class ConditionData
    {
        public abstract ConditionType Type { get; }
        public abstract string Description { get; }
    }
}