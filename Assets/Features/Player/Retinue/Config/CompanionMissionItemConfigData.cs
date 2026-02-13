using System;
using Common.Types;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [Serializable]
    public sealed class CompanionMissionItemConfigData
    {
        [field: SerializeField]
        public int Amount { get; private set; }

        [field: SerializeField]
        public Good Good { get; private set; }
    }
}