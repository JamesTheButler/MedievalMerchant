using Common;
using UnityEngine;

namespace Levels.Conditions
{
    public abstract class Condition : ScriptableObject
    {
        public readonly Observable<bool> IsCompleted = new();
        public abstract ConditionType Type { get; }

        public abstract void Initialize();
    }
}