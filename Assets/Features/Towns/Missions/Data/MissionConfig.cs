using Common.Utility;
using UnityEngine;

namespace Features.Towns.Missions.Data
{
    [CreateAssetMenu(
        fileName = nameof(MissionConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(MissionConfig))]
    public sealed class MissionConfig : ScriptableObject
    {
        [field: SerializeField]
        public TradeMissionConfigData TradeMissionData { get; private set; }

        [field: SerializeField]
        public UpgradeMissionConfigData UpgradeMissionData { get; private set; }

        [field: SerializeField, Min(0)]
        public int WarningThresholdDays { get; private set; } = 7;
    }
}