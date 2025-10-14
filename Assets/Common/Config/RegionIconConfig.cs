using System.Linq;
using AYellowpaper.SerializedCollections;
using Common.Types;
using UnityEngine;

namespace Common.Config
{
    [CreateAssetMenu(fileName = nameof(RegionIconConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(RegionIconConfig))]
    public sealed class RegionIconConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Region", "Icon")]
        public SerializedDictionary<Regions, Sprite> Icons { get; private set; }

        [SerializeField]
        private Sprite fallbackIcon;

        public Sprite GetIcon(Regions regionFlags)
        {
            foreach (var (_, sprite) in Icons.Where(key => regionFlags.HasFlag(key.Key)))
            {
                return sprite;
            }

            return fallbackIcon;
        }
    }
}