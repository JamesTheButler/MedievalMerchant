using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Goods.Config
{
    [Serializable]
    public sealed class AvailabilityResourceData
    {
        [field: SerializeField, ShowAssetPreview]
        public Sprite DefaultIcon { get; private set; }

        [field: SerializeField, ShowAssetPreview]
        public Sprite BuyIcon { get; private set; }

        [field: SerializeField, ShowAssetPreview]
        public Sprite SellIcon { get; private set; }

        [SerializeField]
        private LocalizedString displayString, description;

        public string DisplayString => displayString.GetLocalizedString();
        public string Description => description.GetLocalizedString();
    }
}