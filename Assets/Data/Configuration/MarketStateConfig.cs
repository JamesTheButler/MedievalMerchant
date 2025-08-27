using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = "MarketStateConfig", menuName = AssetMenu.ConfigDataFolder + "MarketStateConfig")]
    public sealed class MarketStateConfig : ScriptableObject
    {
        [SerializedDictionary("Market State", "Config Data")]
        public SerializedDictionary<MarketState, MarketStateConfigData> ConfigData;
    }
}