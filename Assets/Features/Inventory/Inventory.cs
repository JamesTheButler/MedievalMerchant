using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Common.Types;
using Features.Goods.Config;

namespace Features.Inventory
{
    public sealed class Inventory
    {
        public event Action<Good> GoodAdded, GoodRemoved;
        public event Action<Good, int> GoodUpdated;

        public Observable<float> Funds { get; } = new();

        public IInventoryPolicy InventoryPolicy { get; }
        public IReadOnlyDictionary<Good, int> Goods => _goods;

        private readonly Lazy<GoodsResources> _goodsInfoManager = new(() => ResourceManager.Instance.GoodsResources);
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
            if (amount == 0)
                return;

            if (!_goods.ContainsKey(good))
            {
                GoodAdded?.Invoke(good);
            }

            _goods.TryAdd(good, 0);
            _goods[good] += amount;
            GoodUpdated?.Invoke(good, _goods[good]);
        }

        public void RemoveGood(Good good, int amount)
        {
            if (amount == 0) return;
            if (!HasGood(good)) return;

            _goods[good] -= amount;

            if (_goods[good] <= 0)
            {
                _goods.Remove(good);
                GoodRemoved?.Invoke(good);
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

                var tier = _goodsInfoManager.Value.ResourceData[good].Tier;
                result[tier]++;
            }

            return result;
        }
    }
}