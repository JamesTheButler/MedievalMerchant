using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(
        fileName = nameof(GoodResources),
        menuName = AssetMenu.ResourceFolder + nameof(GoodResources))]
    public sealed class GoodResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Good, GoodResourceData> ResourceData { get; private set; }
    }
}