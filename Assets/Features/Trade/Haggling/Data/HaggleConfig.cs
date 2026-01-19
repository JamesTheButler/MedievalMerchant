using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;

namespace Features.Trade.Haggling.Data
{
    [CreateAssetMenu(
        fileName = nameof(HaggleConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(HaggleConfig))]
    public sealed class HaggleConfig : ScriptableObject
    {
        [field: SerializeField]
        public SerializedDictionary<HaggleLevel, HaggleConfigData> Configs { get; private set; }
    }
}