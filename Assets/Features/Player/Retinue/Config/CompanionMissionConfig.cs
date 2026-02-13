using System;
using System.Collections.Generic;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [Serializable]
    public sealed class CompanionMissionConfig
    {
        [field: SerializeField]
        public List<CompanionMissionConfigData> ConfigsPerLevel { get; private set; }
    }
}