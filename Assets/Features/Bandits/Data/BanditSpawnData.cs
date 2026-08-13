using System;
using UnityEngine;

namespace Features.Bandits.Data
{
    [Serializable]
    public sealed class BanditSpawnData
    {
        [field: SerializeField, Range(0f, 1f)]
        public float DailySpawnChanceIncrease { get; private set; } = 0.05f;

        [field: SerializeField]
        public int CooldownAfterSpawnDays { get; private set; } = 5;

        [field: SerializeField]
        public int SpawnTileRadiusMin { get; private set; } = 2;

        [field: SerializeField]
        public int SpawnTileRadiusMax { get; private set; } = 3;

        [field: SerializeField]
        public int StartingUnitCountMin { get; private set; } = 2;

        [field: SerializeField]
        public int StartingUnitCountMax { get; private set; } = 4;

        [field: SerializeField]
        public int StartingGold { get; private set; } = 200;
    }
}
