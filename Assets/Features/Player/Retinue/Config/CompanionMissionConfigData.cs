using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [Serializable]
    public sealed class CompanionMissionConfigData
    {
        [field: SerializeField]
        public int Cost { get; private set; }

        [field: SerializeField]
        public List<CompanionMissionItemConfigData> Items { get; private set; }
    }
}