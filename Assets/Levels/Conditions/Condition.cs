using Common;
using UnityEngine;

namespace Levels.Conditions
{
    public abstract class Condition : ScriptableObject
    {
        public readonly Observable<bool> IsCompleted = new();

        public abstract void Initialize();
    }
}