using System;
using System.Collections.Generic;
using AYellowpaper.SerializedCollections;
using Common.Types;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [Serializable]
    public sealed class CompanionMissionConfigData
    {
        [field: SerializeField]
        public int Cost { get; private set; }

        [field: SerializeField, SerializedDictionary("Good Tier", "Good Amount")]
        public SerializedDictionary<Tier, CompanionMissionTierData> ItemsPerTier { get; private set; }
    }
}