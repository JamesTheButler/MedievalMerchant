using System;
using Features.Player.Retinue.Config.LevelDatas;
using UnityEngine;

namespace Features.Player.Retinue.Config.CompanionDatas
{
    [Serializable]
    public sealed class DiplomatCompanionData : CompanionConfigData<DiplomatLevelData>
    {
        [field: SerializeField]
        public int MinDaysBetweenRepGains { get; private set; } = 3;
    }
}