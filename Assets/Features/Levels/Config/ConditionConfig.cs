using AYellowpaper.SerializedCollections;
using Common;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.Config
{
    [CreateAssetMenu(
        fileName = nameof(ConditionConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(ConditionConfig))]
    public sealed class ConditionConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Type", "Condition Data")]
        public SerializedDictionary<ConditionType, ConditionListItemData> Conditions { get; private set; }

        [field: SerializeField, Range(0f, 1f)]
        public float WarningThresholdPercent { get; private set; }

        [field: SerializeField, Required]
        public Sprite WarningIcon { get; private set; }
    }
}