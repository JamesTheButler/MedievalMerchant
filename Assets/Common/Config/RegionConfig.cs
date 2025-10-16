using AYellowpaper.SerializedCollections;
using Common.Types;
using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(fileName = nameof(RegionConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(RegionConfig))]
    public sealed class RegionConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Region", "Region Info")]
        public SerializedDictionary<Region, RegionConfigData> Data { get; private set; }
    }
}