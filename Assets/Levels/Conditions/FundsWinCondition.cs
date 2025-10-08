using Data;
using Data.Configuration;
using Data.Trade;
using UnityEngine;

namespace Levels.Conditions
{
    [CreateAssetMenu(
        fileName = nameof(FundsWinCondition),
        menuName = AssetMenu.ConditionsFolder + nameof(FundsWinCondition))]
    public sealed class FundsWinCondition : WinCondition
    {
        [SerializeField]
        private int fundsToReach;

        private Inventory _playerInventory;

        public override ConditionType Type => ConditionType.FundsWinCondition;

        public override string Description => $"Accumulate {fundsToReach} gold coins.";

        public override void Initialize()
        {
            _playerInventory = Model.Instance.Player.Inventory;
            Progress = new Progress(fundsToReach, FormatProgress);

            _playerInventory.Funds.Observe(Evaluate);
        }

        private void Evaluate(float funds)
        {
            Progress.SetProgress(Mathf.FloorToInt(funds));
        }

        private static string FormatProgress(int currentValue, int maxValue)
        {
            return $"{currentValue}/{maxValue} coin";
        }
    }
}