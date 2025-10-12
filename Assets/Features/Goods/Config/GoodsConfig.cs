using AYellowpaper.SerializedCollections;
using Common;
using Common.Types;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(fileName = nameof(GoodsConfig), menuName = AssetMenu.ConfigDataFolder + nameof(GoodsConfig))]
    public sealed class GoodsConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Good, GoodConfigData> ConfigData { get; private set; }

        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, float> BasePriceData { get; private set; }

        [field: SerializeField]
        public float ForeignGoodPriceModifier { get; private set; }

        [field: SerializeField]
        public float LocalGoodPriceModifier { get; private set; }
    }
}