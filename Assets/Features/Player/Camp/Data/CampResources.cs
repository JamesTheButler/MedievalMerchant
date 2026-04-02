using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;

namespace Features.Player.Camp.Data
{
    [CreateAssetMenu(
        fileName = nameof(CampResources),
        menuName = AssetMenu.ResourceFolder + nameof(CampResources))]
    public sealed class CampResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Companion Tier", "Frame")]
        public SerializedDictionary<int, Sprite> TierFrames { get; private set; }
    }
}