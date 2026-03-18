using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Towns.Config
{
    [CreateAssetMenu(
        fileName = nameof(TownResources),
        menuName = AssetMenu.ResourceFolder + nameof(TownResources))]
    public sealed class TownResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Region", "Name Generator")]
        public SerializedDictionary<Region, TownNameGenerator> NameGenerators { get; private set; }

        [SerializeField, SerializedDictionary("Town Tier", "Descriptor")]
        private SerializedDictionary<Tier, LocalizedString> townTypeNames;

        public string GetTownDescriptor(Tier tier)
        {
            return townTypeNames[tier].GetLocalizedString();
        }
    }
}