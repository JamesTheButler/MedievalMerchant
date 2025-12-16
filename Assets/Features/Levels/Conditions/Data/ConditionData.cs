using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    public abstract class ConditionData : ScriptableObject
    {
        public abstract ConditionType Type { get; }
        public abstract string Description { get; }
    }
}