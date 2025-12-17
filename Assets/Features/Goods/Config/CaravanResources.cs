using AYellowpaper.SerializedCollections;
using Common;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(
        fileName = nameof(CaravanResources),
        menuName = AssetMenu.ResourceFolder + nameof(CaravanResources))]
    public sealed class CaravanResources : ScriptableObject
    {
        [field: SerializeField, ShowAssetPreview]
        public Sprite DefaultBackgroundImage { get; private set; }
        
        [field: SerializeField, SerializedDictionary("Level", "Cart Images")]
        public SerializedDictionary<int, Sprite> BackgroundImages { get; private set; }
        
        [field: SerializeField, SerializedDictionary("Cart Level", "Cart Tier Icon")]
        public SerializedDictionary<int, Sprite> TierIcons { get; private set; }
    }
}