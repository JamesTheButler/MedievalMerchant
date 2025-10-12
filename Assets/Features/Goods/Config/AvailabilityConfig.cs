using AYellowpaper.SerializedCollections;
using Common;
using Common.Types;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(
        fileName = nameof(AvailabilityConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(AvailabilityConfig))]
    public sealed class AvailabilityConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Availability", "Config Data")]
        public SerializedDictionary<Availability, AvailabilityConfigData> ConfigData { get; private set; }
    }
}