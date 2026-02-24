using System;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;

namespace Common.Config
{
    [Serializable]
    public sealed class RegionResourceData
    {
        [field: SerializeField, Required]
        public Sprite Icon { get; private set; }

        [SerializeField]
        private LocalizedString name;

        public string Name => name.GetLocalizedString();
    }
}