using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class NavigatorCompanionData : CompanionConfigData
    {
        [field: SerializeField]
        public List<NavigatorLevelData> TypedLevels { get; private set; }

        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }
}