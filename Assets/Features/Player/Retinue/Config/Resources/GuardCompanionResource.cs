using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Player.Retinue.Config.Resources
{
    [Serializable]
    public sealed class GuardCompanionResource : CompanionResource
    {
        
        [field: SerializeField]
        public LocalizedString StrengthString { get; private set; }
    }
}