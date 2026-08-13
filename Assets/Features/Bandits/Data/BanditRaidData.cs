using System;
using AYellowpaper.SerializedCollections;
using Common.Types;
using Features.Bandits.Logic;
using UnityEngine;

namespace Features.Bandits.Data
{
    [Serializable]
    public sealed class BanditRaidData
    {
        [field: SerializeField]
        public int TownRecoveringDurationDays { get; private set; } = 7;

        [Header("Raid Duration (days), per Bandit Tier / Town Tier")]
        [SerializeField, SerializedDictionary("Town Tier", "Days")]
        private SerializedDictionary<Tier, int> raidDurationTier1;

        [SerializeField, SerializedDictionary("Town Tier", "Days")]
        private SerializedDictionary<Tier, int> raidDurationTier2;

        [SerializeField, SerializedDictionary("Town Tier", "Days")]
        private SerializedDictionary<Tier, int> raidDurationTier3;

        [SerializeField, SerializedDictionary("Town Tier", "Days")]
        private SerializedDictionary<Tier, int> raidDurationTier4;

        public int? GetRaidDurationDays(BanditTier banditTier, Tier townTier)
        {
            var durationsByTownTier = banditTier switch
            {
                BanditTier.Tier1 => raidDurationTier1,
                BanditTier.Tier2 => raidDurationTier2,
                BanditTier.Tier3 => raidDurationTier3,
                BanditTier.Tier4 => raidDurationTier4,
                _ => throw new ArgumentOutOfRangeException(nameof(banditTier), banditTier, null)
            };

            return durationsByTownTier.TryGetValue(townTier, out var days) ? days : null;
        }
    }
}
