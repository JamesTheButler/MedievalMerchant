using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [CreateAssetMenu(
        fileName = nameof(CompanionResources),
        menuName = AssetMenu.ResourceFolder + nameof(CompanionResources))]
    public sealed class CompanionResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary]
        public SerializedDictionary<CompanionType, CompanionResource> Companions { get; private set; }
    }
}