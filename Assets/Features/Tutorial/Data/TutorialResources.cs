using AYellowpaper.SerializedCollections;
using Common.Utility;
using UnityEngine;

namespace Features.Tutorial.Data
{
    [CreateAssetMenu(
        fileName = nameof(TutorialResources),
        menuName = AssetMenu.ResourceFolder + nameof(TutorialResources))]
    public sealed class TutorialResources : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Topic", "Data")]
        public SerializedDictionary<TutorialTopic, TutorialTopicData> Topics { get; private set; }
    }
}