using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(fileName = nameof(RegionResources),
        menuName = AssetMenu.ResourceFolder + nameof(RegionResources))]
    public sealed class RegionResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Region", "Resources")]
        public SerializedDictionary<Region, RegionResourceData> Data { get; private set; }
    }
}