using AYellowpaper.SerializedCollections;
using Common.Utility;
using Features.Bandits.Logic;
using UnityEngine;

namespace Features.Bandits.Data
{
    [CreateAssetMenu(
        fileName = nameof(BanditConfig),
        menuName = AssetMenu.ConfigDataFolder + nameof(BanditConfig))]
    public sealed class BanditConfig : ScriptableObject
    {
        [field: SerializeField]
        public BanditSpawnData SpawnData { get; private set; }

        [field: SerializeField, SerializedDictionary("Bandit Tier", "Stats")]
        public SerializedDictionary<BanditTier, BanditTierData> TierData { get; private set; }

        [field: SerializeField]
        public BanditCombatData CombatData { get; private set; }

        [field: SerializeField]
        public BanditRestData RestData { get; private set; }

        [field: SerializeField]
        public BanditRushData RushData { get; private set; }

        [field: SerializeField]
        public BanditRaidData RaidData { get; private set; }

        [field: SerializeField, SerializedDictionary("Behavior State", "Effect")]
        public SerializedDictionary<BanditBehaviorState, BanditStateEffectData> StateEffects { get; private set; }

        public BanditTierData GetTierData(BanditTier tier)
        {
            return TierData[tier];
        }

        public BanditStateEffectData GetStateEffect(BanditBehaviorState state)
        {
            return StateEffects[state];
        }
    }
}
