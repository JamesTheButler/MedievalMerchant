using Common;
using Features.Levels.Config.Conditions;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.Config
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
        public string LevelName { get; private set; }

        [field: SerializeField]
        public string Description { get; private set; }

        [field: SerializeField]
        public string Difficulty { get; private set; }

        [field: SerializeField]
        public Color DifficultyColor { get; private set; }

        [field: SerializeField]
        public float StartPlayerFunds { get; private set; }

        [field: SerializeField, Expandable]
        public Condition[] Conditions { get; private set; }

        /// <summary>
        /// Index for internal logic. 0-based.
        /// </summary>
        public int InternalIndex => DisplayIndex - 1;

        /// <summary>
        /// For display purposes for strings like 'Level 03'.
        /// </summary>
        public string LevelNumberText => $"Level {DisplayIndex:D2}";
    }
}