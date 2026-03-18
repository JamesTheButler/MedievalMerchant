using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class PlayerLocalizationResources
    {
        [field: SerializeField]
        public LocalizedString FundsChangeModifier { get; private set; }

        [field: SerializeField]
        public CompanionLocalizationResources Companions { get; private set; }
    }
}