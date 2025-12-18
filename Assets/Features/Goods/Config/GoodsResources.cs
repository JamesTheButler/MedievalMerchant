using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(fileName = nameof(GoodsResources), menuName = AssetMenu.ResourceFolder + nameof(GoodsResources))]
    public sealed class GoodsResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Good, GoodResourceData> ConfigData { get; private set; }
    }
}