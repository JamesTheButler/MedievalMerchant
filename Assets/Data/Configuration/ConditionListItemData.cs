using System;
using UnityEngine;

namespace Data.Configuration
{
    [Serializable]
    public sealed class ConditionListItemData
    {
        [field: SerializeField]
        public string Description { get; private set; }

        [field: SerializeField]
        public Sprite Icon { get; private set; }
        
        public void Deconstruct(out string description, out Sprite icon)
        {
            description = Description;
            icon = Icon;
        }
    }
}