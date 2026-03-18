using System;
using Common.Utility;
using Features.Player.Retinue.Config.Resources;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [CreateAssetMenu(
        fileName = nameof(CompanionResources),
        menuName = AssetMenu.ResourceFolder + nameof(CompanionResources))]
    public sealed class CompanionResources : ScriptableObject
    {
        [field: SerializeField]
        public ArchitectCompanionResource Architect { get; private set; }

        [field: SerializeField]
        public DiplomatCompanionResource Diplomat { get; private set; }

        [field: SerializeField]
        public GuardCompanionResource Guard { get; private set; }

        [field: SerializeField]
        public NavigatorCompanionResource Navigator { get; private set; }

        [field: SerializeField]
        public NegotiatorCompanionResource Negotiator { get; private set; }

        [field: SerializeField]
        public ThiefCompanionResource Thief { get; private set; }

        public CompanionResource Get(CompanionType companionType)
        {
            return companionType switch
            {
                CompanionType.Architect => Architect,
                CompanionType.Diplomat => Diplomat,
                CompanionType.Guard => Guard,
                CompanionType.Navigator => Navigator,
                CompanionType.Negotiator => Negotiator,
                CompanionType.Thief => Thief,
                _ => throw new ArgumentOutOfRangeException(nameof(companionType), companionType, null)
            };
        }
    }
}