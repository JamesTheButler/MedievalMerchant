using Common.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Events.Data
{
    [CreateAssetMenu(
        fileName = nameof(EventConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(EventConfig))]
    public sealed class EventConfig : ScriptableObject
    {
        [field: SerializeField, Range(0f, 1f)]
        public float DailyEventChance { get; private set; }

        [field: SerializeField, Range(1, 365)]
        public int MinDuration { get; private set; }

        [field: SerializeField, Range(1, 365)]
        public int MaxDuration { get; private set; }

        [field: SerializeField]
        public EventSet DefaultEventSet { get; private set; }
    }
}