using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public sealed class ArchitectCompanionData : CompanionConfigData
    {
        [field: SerializeField]
        public List<ArchitectLevelData> TypedLevels { get; private set; }

        public override IReadOnlyList<CompanionLevelData> Levels => TypedLevels;
    }
}