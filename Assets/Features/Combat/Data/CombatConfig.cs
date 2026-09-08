using Common.Utility;
using UnityEngine;

namespace Features.Combat.Data
{
    [CreateAssetMenu(
        fileName = nameof(CombatConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(CombatConfig))]
    public sealed class CombatConfig : ScriptableObject
    {
        [field: SerializeField]
        public float EvenlyMatchedLead { get; private set; } = 0.08f;

        [field: SerializeField]
        public float SlightlyAheadLead { get; private set; } = 0.26f;

        [field: SerializeField]
        public float AheadLead { get; private set; } = 0.55f;

        [field: SerializeField]
        public float AheadMoodLead { get; private set; } = 0.16f;
    }
}