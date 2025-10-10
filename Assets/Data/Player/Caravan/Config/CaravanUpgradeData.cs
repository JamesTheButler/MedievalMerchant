using System;
using NaughtyAttributes;
using UnityEngine;

namespace Data.Player.Caravan.Config
{
    [Serializable]
    public sealed class CaravanUpgradeData
    {
        [field: SerializeField, ShowAssetPreview]
        public Sprite BackgroundImage { get; private set; }

        [field: SerializeField]
        public int UpgradeCost { get; private set; }

        [field: SerializeField]
        public int SlotCount { get; private set; }

        [field: SerializeField]
        public float MoveSpeed { get; private set; }

        [field: SerializeField]
        public int Upkeep { get; private set; }
    }
}