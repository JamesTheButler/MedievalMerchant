using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Configuration;

namespace Data.Trade
{
    public sealed class Inventory
    {
        // TODO - STYLE: should add a "new good added" or "good removed" action
        public event Action<Good, int> GoodUpdated;

        public Observable<float> Funds { get; } = new();

        public IInventoryPolicy InventoryPolicy { get; }
        public IReadOnlyDictionary<Good, int> Goods => _goods;

        private readonly Lazy<GoodsConfig> _goodsInfoManager = new(() => ConfigurationManager.Instance.GoodsConfig);
        private readonly Dictionary<Good, int> _goods = new();

        public Inventory(IInventoryPolicy inventoryPolicy)
        {
            InventoryPolicy = inventoryPolicy;
            inventoryPolicy.SetInventory(this);
        }

        public void AddFunds(float fundChange)
        {
            Funds.Value += fundChange;
        }

        public void RemoveFunds(float fundChange)
        {
            Funds.Value -= fundChange;
        }

        public bool HasFunds(float funds)
        {
            return Funds >= funds;
        }

        public bool HasGood(Good good)
        {
            return _goods.ContainsKey(good);
        }

        public bool HasGood(Good good, int amount)
        {
            return _goods.ContainsKey(good) && _goods[good] >= amount;
        }

        public void AddGood(Good good, int amount)
        {
            _goods.TryAdd(good, 0);
            _goods[good] += amount;
            GoodUpdated?.Invoke(good, _goods[good]);
        }

        public void RemoveGood(Good good, int amount)
        {
            if (!HasGood(good)) return;

            _goods[good] -= amount;

            if (_goods[good] <= 0)
            {
                _goods.Remove(good);
            }

            GoodUpdated?.Invoke(good, _goods.GetValueOrDefault(good, 0));
        }

        public int Get(Good good)
        {
            return _goods.GetValueOrDefault(good, 0);
        }

        public IReadOnlyDictionary<Tier, int> GoodsPerTier()
        {
            var result = Enum.GetValues(typeof(Tier))
                .Cast<Tier>()
                .ToDictionary(tier => tier, _ => 0);

            foreach (var (good, amount) in _goods)
            {
                if (amount <= 0) continue;

                var tier = _goodsInfoManager.Value.ConfigData[good].Tier;
                result[tier]++;
            }

            return result;
        }
    }
}