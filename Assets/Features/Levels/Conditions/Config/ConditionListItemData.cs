using System;
using UnityEngine;

namespace Features.Levels.Conditions.Config
{
    [Serializable]
    public sealed class ConditionListItemData
    {
        [field: SerializeField]
        public Sprite Icon { get; private set; }
    }
}