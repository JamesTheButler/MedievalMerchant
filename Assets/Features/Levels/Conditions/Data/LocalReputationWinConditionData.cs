using System;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [Serializable]
    public sealed class LocalReputationWinConditionData : WinConditionData
    {
        [field: SerializeField]
        public int MinRepPerTown { get; private set; } = 10;

        public override ConditionType Type => ConditionType.LocalRepWinCondition;
        public override string Description => $"Maintain a reputation of {MinRepPerTown} in all towns.";
    }
}