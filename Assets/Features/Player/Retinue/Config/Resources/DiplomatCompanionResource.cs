using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Player.Retinue.Config.Resources
{
    [Serializable]
    public sealed class DiplomatCompanionResource : CompanionResource
    {
        [field: SerializeField]
        public LocalizedString TownEntranceRepString { get; private set; }

        [field: SerializeField]
        public LocalizedString RepBoostString { get; private set; }
    }
}