using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class NegotiatorCompanionData : CompanionConfigData
    {
        [field: SerializeField]
        public List<NegotiatorLevelData> TypedLevels { get; private set; }

        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }
}