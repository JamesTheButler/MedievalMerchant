using System;
using Data;
using Data.Trade;
using UnityEngine;

namespace Levels
{
    [Serializable]
    public sealed class FundsWinCondition : WinCondition
    {
        [SerializeField]
        private int fundsToReach;

        private Inventory _playerInventory;

        private void Awake()
        {
            _playerInventory = Model.Instance.Player.Inventory;
        }

        public override bool Evaluate()
        {
            return fundsToReach >= _playerInventory.Funds;
        }
    }
}