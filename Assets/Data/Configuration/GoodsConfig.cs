using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = nameof(GoodsConfig), menuName = AssetMenu.ConfigDataFolder + nameof(GoodsConfig))]
    public sealed class GoodsConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Good, GoodConfigData> ConfigData { get; private set; }

        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, int> BasePriceData { get; private set; }

        [field: SerializeField]
        public float ForeignGoodPriceModifier { get; private set; }

        [field: SerializeField]
        public float LocalGoodPriceModifier { get; private set; }
    }
}