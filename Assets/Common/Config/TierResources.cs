using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(fileName = nameof(TierResources), menuName = AssetMenu.ResourceFolder + nameof(TierResources))]
    public sealed class TierResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Tier", "Icon")]
        public SerializedDictionary<Tier, Sprite> Icons { get; set; }

        [SerializeField]
        private Sprite tier4Icon;

        public Sprite GetTierIconByLevel(int level)
        {
            return level switch
            {
                <= (int)Tier.Tier1 => Icons[Tier.Tier1],
                (int)Tier.Tier2 => Icons[Tier.Tier2],
                (int)Tier.Tier3 => Icons[Tier.Tier3],
                _ => tier4Icon,
            };
        }
    }
}