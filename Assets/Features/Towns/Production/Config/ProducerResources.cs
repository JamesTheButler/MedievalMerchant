using AYellowpaper.SerializedCollections;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Towns.Production.Config
{
    [CreateAssetMenu(
        fileName = nameof(ProducerResources),
        menuName = AssetMenu.ResourceFolder + nameof(ProducerResources))]
    public sealed class ProducerResources : ScriptableObject
    {
        [SerializeField, SerializedDictionary("Good Tier", "Name for production building")]
        public SerializedDictionary<Good, string> producerNames;
    }
}