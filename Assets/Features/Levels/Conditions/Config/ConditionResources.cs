using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;

namespace Features.Levels.Conditions.Config
{
    [CreateAssetMenu(
        fileName = nameof(ConditionResources),
        menuName = AssetMenu.ResourceFolder + nameof(ConditionResources))]
    public sealed class ConditionResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Type", "Condition Data")]
        public SerializedDictionary<ConditionType, ConditionListItemData> Conditions { get; private set; }
    }
}