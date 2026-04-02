using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Player.Logic;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionDeliveryService : IService
    {
        private PlayerModel _player;
        private Inventory.Inventory _playerInventory;

        public void Initialize()
        {
            _player = GameplayContext.Instance.Model.Player;
            _playerInventory = _player.Inventory;
        }

        public void CleanUp() { }

        public void Substitute(CompanionMissionGoodItem goodMissionItem, int goodAmount)
        {
            var coinAmount = goodAmount * goodMissionItem.SubstituteCostSingle;
            if (!_player.Inventory.HasFunds(coinAmount))
            {
                Debug.LogWarning($"Player does not have {coinAmount} coin to substitute for {goodMissionItem.Good}.");
                return;
            }

            goodMissionItem.Deliver(goodAmount);
            _player.Inventory.RemoveFunds(coinAmount);
        }

        public void Deliver(CompanionMissionItem missionItem, int amount)
        {
            var deliverableAmount = Mathf.Min(amount, missionItem.RemainingAmount.Value);
            if (deliverableAmount <= 0)
                return;

            switch (missionItem)
            {
                case CompanionMissionGoodItem goodMissionItem:
                    if (!_player.Inventory.HasGood(goodMissionItem.Good, deliverableAmount))
                    {
                        Debug.LogWarning($"Player does not have {deliverableAmount}x {goodMissionItem.Good}.");
                        return;
                    }

                    _player.Inventory.RemoveGood(goodMissionItem.Good, deliverableAmount);
                    break;

                case CompanionMissionCoinItem:
                    if (!_playerInventory.HasFunds(deliverableAmount))
                    {
                        Debug.LogWarning($"Player does not have {deliverableAmount} funds.");
                        return;
                    }

                    _player.Inventory.RemoveFunds(deliverableAmount);
                    break;
            }

            missionItem.Deliver(amount);
        }
    }
}