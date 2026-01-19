using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;

namespace Features.Trade.Haggling.Data
{
    [CreateAssetMenu(
        fileName = nameof(HaggleResources),
        menuName = AssetMenu.ResourceFolder + nameof(HaggleResources))]
    public sealed class HaggleResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<HaggleLevel, string> HaggleLevelNames { get; private set; }
    }
}