using System;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [Serializable]
    public abstract class EffectData
    {
        public abstract string Description { get; }
    }
}