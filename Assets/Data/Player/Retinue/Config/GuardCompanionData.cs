using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class GuardCompanionData : CompanionConfigData
    {
        [field: SerializeField]
        public List<GuardLevelData> TypedLevels { get; private set; }

        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }
}