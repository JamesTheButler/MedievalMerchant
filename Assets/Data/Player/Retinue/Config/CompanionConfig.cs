using System;
using Data.Configuration;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [CreateAssetMenu(
        fileName = nameof(CompanionConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(CompanionConfig))]
    public sealed class CompanionConfig : ScriptableObject
    {
        [field: SerializeField] public ThiefCompanionData ThiefData { get; private set; }
        [field: SerializeField] public NavigatorCompanionData NavigatorData { get; private set; }
        [field: SerializeField] public NegotiatorCompanionData NegotiatorData { get; private set; }
        [field: SerializeField] public GuardCompanionData GuardData { get; private set; }
        [field: SerializeField] public DiplomatCompanionData DiplomatData { get; private set; }
        [field: SerializeField] public ArchitectCompanionData ArchitectData { get; private set; }

        public CompanionConfigData<TLevelData> Get<TLevelData>(CompanionType companionType)
            where TLevelData : CompanionLevelData
        {
            return companionType switch
            {
                CompanionType.Thief => ThiefData as CompanionConfigData<TLevelData>,
                CompanionType.Navigator => NavigatorData as CompanionConfigData<TLevelData>,
                CompanionType.Negotiator => NegotiatorData as CompanionConfigData<TLevelData>,
                CompanionType.Guard => GuardData as CompanionConfigData<TLevelData>,
                CompanionType.Diplomat => DiplomatData as CompanionConfigData<TLevelData>,
                CompanionType.Architect => ArchitectData as CompanionConfigData<TLevelData>,
                _ => throw new ArgumentOutOfRangeException(nameof(companionType), companionType, null)
            };
        }
    }
}