using System;
using System.Collections.Generic;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class DiplomatCompanionData : CompanionConfigData
    {
        [field: SerializeField]
        public List<DiplomatLevelData> TypedLevels { get; private set; }

        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }
}