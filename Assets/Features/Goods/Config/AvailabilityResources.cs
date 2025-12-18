using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(
        fileName = nameof(AvailabilityResources),
        menuName = AssetMenu.ResourceFolder + nameof(AvailabilityResources))]
    public sealed class AvailabilityResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Availability", "Resources")]
        public SerializedDictionary<Availability, AvailabilityResourceData> Resources { get; private set; }
    }
}