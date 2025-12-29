using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Towns.Config
{
    [CreateAssetMenu(
        fileName = nameof(TownResources),
        menuName = AssetMenu.ResourceFolder + nameof(TownResources))]
    public sealed class TownResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Region", "Name Generator")]
        public SerializedDictionary<Region, TownNameGenerator> NameGenerators { get; private set; }

        [field: SerializeField, SerializedDictionary("Town Tier", "Descriptor")]
        public SerializedDictionary<Tier, string> TownTypeNames { get; private set; }
    }
}