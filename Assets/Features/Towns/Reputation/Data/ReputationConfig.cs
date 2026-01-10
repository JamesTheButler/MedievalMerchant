using Common.Utility;
using UnityEngine;

namespace Features.Towns.Reputation.Data
{
    [CreateAssetMenu(
        fileName = nameof(ReputationConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(ReputationConfig))]
    public sealed class ReputationConfig : ScriptableObject
    {
        [field: SerializeField]
        public NeglectData NeglectData { get; private set; }

        [field: SerializeField]
        public ReputationRewardData RewardData { get; private set; }

        [field: SerializeField]
        public int ReputationPerPricePercent { get; private set; } = 10;
    }
}