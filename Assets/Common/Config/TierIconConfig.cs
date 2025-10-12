using AYellowpaper.SerializedCollections;
using Common.Types;
using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(fileName = nameof(TierIconConfig), menuName = AssetMenu.ConfigDataFolder + nameof(TierIconConfig))]
    public sealed class TierIconConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Tier", "Icon")]
        public SerializedDictionary<Tier, Sprite> Icons { get; set; }
    }
}