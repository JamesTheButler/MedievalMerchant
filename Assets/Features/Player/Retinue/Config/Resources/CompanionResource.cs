using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Player.Retinue.Config.Resources
{
    [Serializable]
    public abstract class CompanionResource
    {
        [field: SerializeField, Required, ShowAssetPreview]
        public Sprite Icon { get; private set; }

        [SerializeField]
        private LocalizedString name, description;

        public string Name => name.GetLocalizedString();
        public string Description => description.GetLocalizedString();
    }
}