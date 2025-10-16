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
        public SerializedDictionary<Region, Sprite> Icons { get; private set; }

        [SerializeField]
        private Sprite fallbackIcon;
    }
}