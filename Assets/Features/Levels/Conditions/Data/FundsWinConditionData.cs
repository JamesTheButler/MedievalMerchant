using Common;
using UnityEngine;

namespace Features.Levels.Conditions.Data
{
    [CreateAssetMenu(
        fileName = nameof(FundsWinConditionData),
        menuName = AssetMenu.ConditionsFolder + nameof(FundsWinConditionData))]
    public sealed class FundsWinConditionData : WinConditionData
    {
        [field: SerializeField]
        public int FundsToReach { get; private set; } = 9999;

        public override ConditionType Type => ConditionType.FundsWinCondition;
        public override string Description => $"Accumulate {FundsToReach} coin.";
    }
}