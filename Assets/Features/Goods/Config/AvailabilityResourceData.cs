using System;
using NaughtyAttributes;
using UnityEngine;

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

        [field: SerializeField]
        public string DisplayString { get; private set; }

        [field: SerializeField]
        public string Description { get; private set; }
    }
}