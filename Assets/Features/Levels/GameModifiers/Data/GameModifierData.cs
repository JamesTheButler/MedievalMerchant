using System.Collections.Generic;
using Features.Levels.GameModifiers.Effects.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.GameModifiers.Data
{
    public abstract class GameModifierData : ScriptableObject
    {
        [field: SerializeField]
        public string Title { get; private set; }

        [field: SerializeField, TextArea]
        public string Description { get; private set; }

        [field: SerializeField]
        public List<EffectData> Effects { get; private set; }
    }
}