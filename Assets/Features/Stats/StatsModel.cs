using System.Collections.Generic;
using Common.Types;

namespace Features.Stats
{
    public sealed class StatsModel
    {
        public int TradesAborted { get; set; }
        public int TradesCompleted { get; set; }
        public float TotalValueTraded => TotalValueBought + TotalValueSold;
        public float TotalValueBought { get; set; }
        public float TotalValueSold { get; set; }

        public int TotalGoodsSold { get; private set; }
        public int TotalGoodsBought { get; private set; }
        public int TotalGoodsTraded => TotalGoodsBought + TotalGoodsSold;

        public IReadOnlyDictionary<Good, int> SoldGoods => _soldGoods;
        public IReadOnlyDictionary<Good, int> BoughtGoods => _boughtGoods;
        public IReadOnlyDictionary<Good, int> TradedGoods => _tradedGoods;
        private readonly Dictionary<Good, int> _soldGoods = new();
        private readonly Dictionary<Good, int> _boughtGoods = new();
        private readonly Dictionary<Good, int> _tradedGoods = new();

        public void TrackSoldGood(Good good, int amount)
        {
            TotalGoodsSold += amount;
            _soldGoods.TryAdd(good, 0);
            _tradedGoods.TryAdd(good, 0);
            _soldGoods[good] += amount;
            _tradedGoods[good] += amount;
        }

        public void TrackBoughtGood(Good good, int amount)
        {
            TotalGoodsBought += amount;
            _boughtGoods.TryAdd(good, 0);
            _tradedGoods.TryAdd(good, 0);
            _boughtGoods[good] += amount;
            _tradedGoods[good] += amount;
        }
    }
}