using System.Collections.Generic;
using Common.Types;
using UnityEngine;

namespace Features.Player.Logic
{
    public sealed class TradeTracker
    {
        public IReadOnlyDictionary<Good, TradeTrackInfo> TrackedGoods => _trackedGoods;

        private readonly Dictionary<Good, TradeTrackInfo> _trackedGoods = new();

        public void Add(Good good, int amount, float totalPrice)
        {
            var existingInfo = _trackedGoods.GetValueOrDefault(good);
            var newCount = (existingInfo?.Amount ?? 0) + amount;
            var newTotal = (existingInfo?.TotalPrice ?? 0) + totalPrice;
            Update(good, newCount, newTotal);

            Debug.Log($"Tracked {amount}x{good} for {totalPrice} to new amount ({newCount})/total({newTotal})");
        }

        public void Remove(Good good, int amount)
        {
            if (!_trackedGoods.TryGetValue(good, out var trackedInfo))
                return;

            if (trackedInfo.Amount <= amount)
            {
                _trackedGoods.Remove(good);
                return;
            }

            var newTotal = trackedInfo.TotalPrice - amount * trackedInfo.AveragePrice;
            var newCount = trackedInfo.Amount - amount;
            Update(good, newCount, newTotal);

            Debug.Log($"Removed {amount}x{good} for to new amount ({newCount})/total: ({newTotal})");
        }

        private void Update(Good good, int amount, float totalPrice)
        {
            if (amount <= 0)
            {
                _trackedGoods.Remove(good);
            }
            else
            {
                _trackedGoods[good] = new TradeTrackInfo(amount, totalPrice);
            }
        }
    }
}