using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Tutorial.Data
{
    [Serializable]
    public sealed class TutorialChapterData
    {
        [SerializeField]
        private LocalizedString title, description;

        [field: SerializeField, ShowAssetPreview(128, 128)]
        public Texture2D Image { get; private set; }

        public string Title => title.GetLocalizedString();
        public string Description => description.GetLocalizedString();
    }
}