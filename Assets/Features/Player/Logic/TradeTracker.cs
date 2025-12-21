using System.Collections.Generic;
using Common.Types;

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

            _trackedGoods[good] = new TradeTrackInfo(newCount, newTotal);
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
            _trackedGoods[good] = new TradeTrackInfo(trackedInfo.Amount - amount, newTotal);
        }
    }
}