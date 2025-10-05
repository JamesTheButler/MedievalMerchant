using System;
using UnityEngine;

namespace Data.Player.Retinue.Config
{
    [Serializable]
    public abstract class CompanionLevelData
    {
        [field: SerializeField] public float Cost { get; private set; }

        public abstract string Description { get; }
    }
}