using System;
using UnityEngine;

namespace Features.Player.Retinue.Config
{
    [Serializable]
    public sealed class CompanionMissionTierData
    {
        [field: SerializeField]
        public int AmountOfDifferentGoods { get; private set; }

        [field: SerializeField]
        public int CountPerGood { get; private set; }
    }
}