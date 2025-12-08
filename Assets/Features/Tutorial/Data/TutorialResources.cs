using Common;
using UnityEngine;
using UnityEngine.Rendering;

namespace Features.Tutorial.Data
{
    [CreateAssetMenu(
        fileName = nameof(TutorialResources),
        menuName = AssetMenu.ResourceFolder + nameof(TutorialResources))]
    public sealed class TutorialResources : ScriptableObject
    {
        [field: SerializeField]
        public SerializedDictionary<TutorialTopic, TutorialTopicData> Topics { get; private set; }
    }
}