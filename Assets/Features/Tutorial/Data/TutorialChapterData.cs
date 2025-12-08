using System;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Tutorial.Data
{
    [Serializable]
    public sealed class TutorialChapterData
    {
        [field: SerializeField]
        public string Title { get; private set; }

        [field: SerializeField]
        public string Description { get; private set; }

        [field: SerializeField, ShowAssetPreview(128,128)]
        public Texture2D Image { get; private set; }
    }
}