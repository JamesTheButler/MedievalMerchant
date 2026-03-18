using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Player.Retinue.Config.Resources
{
    [Serializable]
    public sealed class NavigatorCompanionResource : CompanionResource
    {
        [field: SerializeField]
        public LocalizedString SpeedBonusString { get; private set; }

        [field: SerializeField]
        public LocalizedString UpkeepReductionString { get; private set; }
    }
}