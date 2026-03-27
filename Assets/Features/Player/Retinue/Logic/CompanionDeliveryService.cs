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
        private RetinueModel _retinueModel;
        private Inventory.Inventory _playerInventory;

        public void Initialize()
        {
            _player = GameplayContext.Instance.Model.Player;
            _playerInventory = _player.Inventory;
            _retinueModel = _player.RetinueModel;
        }

        public void CleanUp() { }

        public void DeliverGood(CompanionType companionType, Good good, int amount)
        {
            if (amount <= 0)
                return;

            var companion = _retinueModel.Companions[companionType];
            var mission = companion.ActiveMission.Value;

            if (mission == null)
            {
                Debug.LogWarning($"No active mission for {companionType}.");
                return;
            }

            if (!mission.MissionItems.TryGetValue(good, out var missionItem))
            {
                Debug.LogWarning($"Mission for {companionType} does not require {good}.");
                return;
            }

            if (missionItem.IsCompleted.Value)
                return;

            var deliverAmount = Mathf.Min(amount, missionItem.RemainingAmount.Value);
            if (deliverAmount <= 0)
                return;

            if (!_player.Inventory.HasGood(good, deliverAmount))
            {
                Debug.LogWarning($"Player does not have {deliverAmount}x {good}.");
                return;
            }

            _playerInventory.RemoveGood(good, deliverAmount);
            mission.Deliver(good, deliverAmount);
        }

        public void DeliverCoin(CompanionType companionType, int amount)
        {
            if (amount <= 0)
                return;

            var companion = _retinueModel.Companions[companionType];
            var mission = companion.ActiveMission.Value;

            if (mission == null)
            {
                Debug.LogWarning($"No active mission for {companionType}.");
                return;
            }

            if (mission.CoinCost.IsCompleted.Value)
                return;

            var deliverAmount = Mathf.Min(amount, mission.CoinCost.RemainingAmount.Value);
            if (deliverAmount <= 0)
                return;

            if (!_playerInventory.HasFunds(deliverAmount))
            {
                Debug.LogWarning($"Player does not have {deliverAmount} funds.");
                return;
            }

            _playerInventory.RemoveFunds(deliverAmount);
            mission.DeliverCoin(deliverAmount);
        }
    }
}