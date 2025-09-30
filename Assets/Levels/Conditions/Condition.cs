using UnityEngine;

namespace Levels.Conditions
{
    public abstract class Condition : ScriptableObject
    {
        public abstract ConditionType Type { get; }
        public Progress Progress { get; protected set; }
        public abstract string Description { get; }

        public abstract void Initialize();
    }
}