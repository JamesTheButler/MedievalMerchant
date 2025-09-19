using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;

namespace Levels
{
    [CreateAssetMenu(fileName = nameof(LevelInfo), menuName = AssetMenu.ConfigDataFolder + nameof(LevelInfo))]
    public sealed class LevelInfo : ScriptableObject
    {
        [field: SerializeField, Required]
        public GameObject MapPrefab { get; private set; }

        [field: SerializeField]
        public string LevelName { get; private set; }

        [field: SerializeField]
        public WinCondition[] WinConditions { get; private set; }
        
        [field: SerializeField]
        public LossCondition[] LossConditions { get; private set; }
    }
}