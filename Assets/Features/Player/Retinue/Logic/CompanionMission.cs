using System.Collections.Generic;
using Common.Infrastructure.Observation;
using Common.Types;
using UnityEngine;

namespace Features.Player.Retinue.Logic
{
    public sealed class CompanionMission
    {
        public CompanionMissionItem CoinCost { get; }
        public Dictionary<Good, CompanionMissionItem> MissionItems { get; } = new();
        public ObservableEvent Completed { get; } = new();

        private int _incompleteItemCount;

        public CompanionMission(int cost, IReadOnlyDictionary<Good, int> goods)
        {
            CoinCost = new CompanionMissionCoinItem(cost);
            CoinCost.IsCompleted.Observe(OnMissionCompleted, false);
            _incompleteItemCount++;

            foreach (var (good, amount) in goods)
            {
                var item = new CompanionMissionGoodItem(good, amount);

                MissionItems.Add(good, item);
                item.IsCompleted.Observe(OnMissionCompleted, false);
                _incompleteItemCount++;
            }
        }

        public void Deliver(Good good, int amount)
        {
            if (!MissionItems.TryGetValue(good, out var item))
            {
                Debug.LogWarning($"This companion mission does not required good '{good}'.");
                return;
            }

            item.Deliver(amount);
        }

        public void DeliverCoin(int coinAmount)
        {
            CoinCost.Deliver(coinAmount);
        }

        private void OnMissionCompleted(bool isComplete)
        {
            if (!isComplete)
                return;
            _incompleteItemCount--;

            if (_incompleteItemCount > 0)
                return;

            Completed.Invoke();
        }
    }
}