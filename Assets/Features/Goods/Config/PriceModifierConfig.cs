using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(
        fileName = nameof(PriceModifierConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(PriceModifierConfig))]
    public sealed class PriceModifierConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Availability", "Config Data")]
        public SerializedDictionary<Availability, AvailabilityConfigData> AvailabilityConfigData { get; private set; }

        [field: SerializeField]
        public GlobalSurplusModiferConfigData GlobalSurplusModiferConfig { get; private set; }

        [field: SerializeField]
        public DisinterestModiferConfigData DisinterestModiferConfig { get; private set; }
    }
}