using AYellowpaper.SerializedCollections;
using Levels.Conditions;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(
        fileName = nameof(ConditionConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(ConditionConfig))]
    public sealed class ConditionConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Type", "Condition Data")]
        public SerializedDictionary<ConditionType, ConditionListItemData> Conditions { get; private set; }
    }
}