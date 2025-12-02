using System;
using NaughtyAttributes;
using UnityEngine;

namespace Common.Config
{
    [Serializable]
    public sealed class RegionResourceData
    {
        [field: SerializeField, Required]
        public Sprite Icon { get; private set; }

        [field: SerializeField]
        public string Name { get; private set; }
    }
}