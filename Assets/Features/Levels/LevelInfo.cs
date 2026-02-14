using System.Collections.Generic;
using Common.Types;
using Common.Utility;
using Features.Levels.Conditions.Data;
using Features.Levels.GameModifiers.Data;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Levels
{
    [CreateAssetMenu(fileName = nameof(LevelInfo), menuName = AssetMenu.ConfigDataFolder + nameof(LevelInfo))]
    public sealed class LevelInfo : ScriptableObject
    {
        [field: SerializeField]
        public bool IsEnabled { get; private set; }

        /// <summary>
        /// Index for display to the player. 1-based.
        /// </summary>
        [field: SerializeField]
        public int DisplayIndex { get; private set; }

        [field: SerializeField, Required]
        public GameObject MapPrefab { get; private set; }

        [field: SerializeField]
        public LocalizedString LevelName { get; private set; }

        [field: SerializeField]
        public LocalizedString Description { get; private set; }

        [field: SerializeField]
        public Difficulty Difficulty { get; private set; }

        [field: SerializeField]
        public float StartPlayerFunds { get; private set; }

        [field: SerializeReference, SubclassSelector]
        public List<ConditionData> Conditions { get; private set; }

        [field: SerializeField, Expandable, Required]
        public LevelGameModifierData GameplayModifiers { get; private set; }

        [field: SerializeField]
        public int StartTownIndex { get; private set; } = -1;

        [SerializeField]
        private LevelFeatureFlags levelFeatures;

        /// <summary>
        /// Index for internal logic. 0-based.
        /// </summary>
        public int InternalIndex => DisplayIndex - 1;

        public bool HasFeature(LevelFeatureFlags flags)
        {
            return (levelFeatures & flags) != 0;
        }
    }
}