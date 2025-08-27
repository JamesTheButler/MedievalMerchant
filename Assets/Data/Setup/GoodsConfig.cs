using AYellowpaper.SerializedCollections;
using UnityEngine;
using UnityEngine.Serialization;

namespace Data.Setup
{
    
    [CreateAssetMenu(fileName = "GoodInfoManager", menuName = "Data/GoodInfoManager")]
    public sealed class GoodsConfig : ScriptableObject
    {
        [FormerlySerializedAs("GoodInfos"),SerializedDictionary]
        public SerializedDictionary<Good, GoodConfigData> ConfigData;
    }
}