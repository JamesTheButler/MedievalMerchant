using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
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