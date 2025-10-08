using System;
using Data.Configuration;
using Data.Player.Retinue.Config.CompanionDatas;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [CreateAssetMenu(
        fileName = nameof(CompanionConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(CompanionConfig))]
    public sealed class CompanionConfig : ScriptableObject
    {
        [field: SerializeField]
        public ThiefCompanionData ThiefData { get; private set; }

        [field: SerializeField]
        public NavigatorCompanionData NavigatorData { get; private set; }

        [field: SerializeField]
        public NegotiatorCompanionData NegotiatorData { get; private set; }

        [field: SerializeField]
        public GuardCompanionData GuardData { get; private set; }

        [field: SerializeField]
        public DiplomatCompanionData DiplomatData { get; private set; }

        [field: SerializeField]
        public ArchitectCompanionData ArchitectData { get; private set; }

        public CompanionConfigData Get(CompanionType companionType)
        {
            return companionType switch
            {
                CompanionType.Thief => ThiefData,
                CompanionType.Navigator => NavigatorData,
                CompanionType.Negotiator => NegotiatorData,
                CompanionType.Guard => GuardData,
                CompanionType.Diplomat => DiplomatData,
                CompanionType.Architect => ArchitectData,
                _ => throw new ArgumentOutOfRangeException(nameof(companionType), companionType, null)
            };
        }
    }
}