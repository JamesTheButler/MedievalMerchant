using AYellowpaper.SerializedCollections;
using Data.Configuration;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = nameof(GoodsConfig), menuName = AssetMenu.ConfigDataFolder + nameof(GoodsConfig))]
    public sealed class GoodsConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Good, GoodConfigData> ConfigData { get; private set; }
        
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Tier, int> BasePriceData { get; private set; }
    }
}