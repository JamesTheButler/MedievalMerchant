using System;
using UnityEngine;

namespace Data.Configuration
{
    [Serializable]
    public sealed class ConditionListItemData
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }
    }
}