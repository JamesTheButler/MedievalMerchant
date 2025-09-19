using UnityEngine;

namespace Levels
{
    public abstract class WinCondition : ScriptableObject
    {
        public abstract bool Evaluate();
    }
}