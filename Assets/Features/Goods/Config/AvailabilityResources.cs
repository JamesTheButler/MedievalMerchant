using AYellowpaper.SerializedCollections;
using Common;
using Common.Types;
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