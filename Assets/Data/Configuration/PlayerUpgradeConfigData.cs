using System;
using UnityEngine;

namespace Data.Configuration
{
    [Serializable]
    public sealed class PlayerUpgradeConfigData
    {
        [field: SerializeField]
        public int Price { get; private set; }

        [field: SerializeField]
        public Tier Tier { get; private set; }

        [field: SerializeField]
        public int SlotCount { get; private set; }
    }
}