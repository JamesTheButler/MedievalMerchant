using System;
using Features.Player.Retinue.Config.LevelDatas;
using UnityEngine;

namespace Features.Player.Retinue.Config.CompanionDatas
{
    [Serializable]
    public sealed class ThiefCompanionData : CompanionConfigData<ThiefLevelData>
    {
        [field: SerializeField]
        public int MinDaysBetweenThefts { get; private set; } = 5;
    }
}