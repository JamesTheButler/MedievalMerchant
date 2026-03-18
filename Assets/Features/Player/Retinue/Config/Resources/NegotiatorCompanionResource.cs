using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Player.Retinue.Config.Resources
{
    [Serializable]
    public sealed class NegotiatorCompanionResource : CompanionResource
    {
        
        [field: SerializeField]
        public LocalizedString PriceSavingsString { get; private set; }

        [field: SerializeField]
        public LocalizedString UpgradeCostReductionString { get; private set; }
    }
}