using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = "MarketStateConfig", menuName = AssetMenu.ConfigDataFolder + "MarketStateConfig")]
    public sealed class MarketStateConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Market State", "Config Data")]
        public SerializedDictionary<MarketState, MarketStateConfigData> ConfigData { get; private set; }
    }
}