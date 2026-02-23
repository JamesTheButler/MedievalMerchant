using System.Collections.Generic;
using Common.Utility;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Tutorial.Data
{
    [CreateAssetMenu(
        fileName = nameof(TutorialTopicData),
        menuName = AssetMenu.ResourceFolder + nameof(TutorialTopicData))]
    public sealed class TutorialTopicData : ScriptableObject
    {
        [SerializeField]
        private LocalizedString title;

        [field: SerializeField]
        public List<TutorialChapterData> Chapters { get; private set; }

        public string Title => title.GetLocalizedString();
    }
}