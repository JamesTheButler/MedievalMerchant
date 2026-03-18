using System;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Player.Retinue.Config.Resources
{
    [Serializable]
    public sealed class ArchitectCompanionResource : CompanionResource
    {
        [field: SerializeField]
        public LocalizedString CostReductionString { get; private set; }
    }
}