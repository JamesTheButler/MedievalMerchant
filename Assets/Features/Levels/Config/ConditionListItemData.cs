using System;
using UnityEngine;

namespace Features.Levels.Config
{
    [Serializable]
    public sealed class ConditionListItemData
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }
    }
}