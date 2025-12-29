using Common.Utility;
using UnityEngine;

namespace Features.Ticking.Config
{
    [CreateAssetMenu(
        fileName = nameof(TickConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(TickConfig))]
    public sealed class TickConfig : ScriptableObject
    {
        [field: SerializeField, Min(1)]
        public int TicksPerDay { get; private set; } = 10;

        [field: SerializeField, Min(0.1f)]
        public float SecondsPerDayDefault { get; private set; } = 2.5f;
        
        [field: SerializeField, Min(0.1f)]
        public float SecondsPerDayFast { get; private set; } = 2.5f;
    }
}