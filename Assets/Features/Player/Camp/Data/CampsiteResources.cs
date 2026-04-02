using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;

namespace Features.Player.Camp.Data
{
    [CreateAssetMenu(
        fileName = nameof(CampsiteResources),
        menuName = AssetMenu.ResourceFolder + nameof(CampsiteResources))]
    public sealed class CampsiteResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Companion Tier", "Frame")]
        public SerializedDictionary<int, Sprite> TierFrames { get; private set; }
    }
}