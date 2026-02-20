using System.Collections.Generic;
using Common.Utility;
using Features.Levels.GameModifiers.Effects.Data;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Levels.GameModifiers.Data
{
    public abstract class GameModifierData : ScriptableObject
    {
        [SerializeField]
        private LocalizedString title, description;

        public string Title => title.GetLocalizedString();
        public string Description => description.GetLocalizedString();

        [field: SerializeReference, SubclassSelector]
        public List<EffectData> Effects { get; private set; }

        public string EffectsString => Effects.AggregateString(effect => $"- {effect.Description}\n");
    }
}