using Common;
using Features.Levels.Config.Conditions;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.Config
{
    [CreateAssetMenu(fileName = nameof(LevelInfo), menuName = AssetMenu.ConfigDataFolder + nameof(LevelInfo))]
    public sealed class LevelInfo : ScriptableObject
    {
        [field: SerializeField, Required]
        public GameObject MapPrefab { get; private set; }

        [field: SerializeField]
        public string LevelName { get; private set; }

        [field: SerializeField]
        public float StartPlayerFunds { get; private set; }

        [field: SerializeField, Expandable]
        public Condition[] Conditions { get; private set; }
    }
}