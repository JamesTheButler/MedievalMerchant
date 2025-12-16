using Features.Levels.Conditions.Model;
using Infrastructure;
using UnityEngine;

namespace Features.Levels.Conditions.Logic
{
    public sealed class FundsWinConditionLogic : IConditionLogic
    {
        private readonly FundsWinCondition _condition;

        private Inventory.Inventory _playerInventory;

        public FundsWinConditionLogic(FundsWinCondition condition)
        {
            _condition = condition;
        }

        public void Initialize()
        {
            _playerInventory = GameplayContext.Instance.Model.Player.Inventory;
            _playerInventory.Funds.Observe(Evaluate, false);
        }

        public void CleanUp()
        {
            _playerInventory.Funds.StopObserving(Evaluate);
        }

        private void Evaluate(float funds)
        {
            _condition.Progress.SetProgress(Mathf.FloorToInt(funds));
        }
    }
}