using System;
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
        public override string Description { get; protected set; }

        private void Awake()
        {
            Description = $"Accumulate {fundsToReach} gold coins.";
        }

        public override void Initialize()
        {
            _playerInventory = Model.Instance.Player.Inventory;
            _playerInventory.Funds.Observe(Evaluate);
        }

        private void Evaluate(int funds)
        {
            IsCompleted.Value = fundsToReach <= _playerInventory.Funds;
        }
    }
}