using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    public abstract class EffectData : ScriptableObject
    {
        public abstract string Description { get; }
    }
}