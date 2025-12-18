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
        [field: SerializeField, SerializedDictionary("Region", "Name Generator"), Header("Town Setup")]
        public SerializedDictionary<Region, TownNameGenerator> NameGenerators { get; private set; }
    }
}