using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Features.Tutorial.Data
{
    [CreateAssetMenu(
        fileName = nameof(TutorialTopicData),
        menuName = AssetMenu.ResourceFolder + nameof(TutorialTopicData))]
    public sealed class TutorialTopicData : ScriptableObject
    {
        [field: SerializeField]
        public string Title { get; private set; }
        
        [field: SerializeField]
        public List<TutorialChapterData> Chapters { get; private set; }
    }
}