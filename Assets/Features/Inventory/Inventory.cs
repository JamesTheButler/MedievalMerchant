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

        public IReadOnlyObservableEvent<Good, int, int> GoodUpdatedWithOld => _goodUpdatedWithOld;
        public Observable<float> Funds { get; } = new();

        public IInventoryPolicy InventoryPolicy { get; }
        public IReadOnlyDictionary<Good, int> Goods => _goods;

        private readonly ObservableEvent<Good, int, int> _goodUpdatedWithOld = new();
        private readonly Lazy<GoodResources> _goodsInfoManager = new(() => ResourceManager.Instance.GoodResources);
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
                _goods.Add(good, 0);
            }

            var oldValue = _goods[good];
            var newValue = oldValue + amount;

            _goods[good] = newValue;
            GoodUpdated?.Invoke(good, newValue);
            _goodUpdatedWithOld.Invoke(good, oldValue, newValue);
        }

        public void RemoveGood(Good good, int amount)
        {
            if (amount == 0) return;
            if (!HasGood(good)) return;

            var oldValue = _goods[good];
            var newValue = Math.Max(oldValue - amount, 0);

            _goods[good] = newValue;

            if (_goods[good] <= 0)
            {
                _goods.Remove(good);
                GoodRemoved?.Invoke(good);
            }

            GoodUpdated?.Invoke(good, newValue);
            _goodUpdatedWithOld.Invoke(good, oldValue, newValue);
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