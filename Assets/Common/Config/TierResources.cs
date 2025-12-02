using AYellowpaper.SerializedCollections;
using Common.Types;
using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(fileName = nameof(TierResources), menuName = AssetMenu.ResourceFolder + nameof(TierResources))]
    public sealed class TierResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Tier", "Icon")]
        public SerializedDictionary<Tier, Sprite> Icons { get; set; }
    }
}