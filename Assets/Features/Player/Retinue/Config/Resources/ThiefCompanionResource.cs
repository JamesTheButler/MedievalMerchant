using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Player.Retinue.Config.Resources
{
    [Serializable]
    public sealed class ThiefCompanionResource : CompanionResource
    {
        
        [field: SerializeField]
        public LocalizedString TownEntranceGoldString { get; private set; }

        [field: SerializeField]
        public LocalizedString ReputationLossChanceString { get; private set; }

        [field: SerializeField]
        public LocalizedString ReputationLossString { get; private set; }
    }
}