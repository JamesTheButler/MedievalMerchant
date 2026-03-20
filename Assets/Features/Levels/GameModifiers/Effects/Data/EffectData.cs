using System;
using Common.Infrastructure;
using Features.Localization.Data;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [Serializable]
    public abstract class EffectData
    {
        protected ModifierLocalizationResources Loc => ResourceManager.Instance.LocalizationResources.Modifiers;

        public abstract string Description { get; }
    }
}