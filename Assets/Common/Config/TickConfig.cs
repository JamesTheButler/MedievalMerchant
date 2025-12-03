using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(
        fileName = nameof(TickConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(TickConfig))]
    public sealed class TickConfig : ScriptableObject
    {
        [field: SerializeField, Min(0.05f)]
        public float SecondsPerTick  { get; private set; } = .1f;
    
        [field: SerializeField, Min(1)]
        public int TicksPerDay { get; private set; }
    }
}