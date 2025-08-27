using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Setup
{
    [CreateAssetMenu(fileName = "GoodInfoManager", menuName = "Data/GoodInfoManager")]
    public sealed class GoodsConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<Good, GoodConfigData> ConfigData { get; private set; }
    }
}