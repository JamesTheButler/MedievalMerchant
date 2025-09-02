using System;
using System.Collections.Generic;
using System.Linq;
using Data.Configuration;
using Data.Setup;

namespace Data
{
    public class Inventory
    {
        public event Action<int> FundsUpdated;
        public event Action<Good, int> GoodUpdated;

        public int Funds { get; private set; }

        public IReadOnlyDictionary<Good, int> Goods => _goods;

        protected readonly Lazy<GoodsConfig> GoodsInfoManager = new(() => ConfigurationManager.Instance.GoodsConfig);
        private readonly Dictionary<Good, int> _goods = new();


        public void AddFunds(int fundChange)
        {
            Funds += fundChange;
            FundsUpdated?.Invoke(Funds);
        }

        public void RemoveFunds(int fundChange)
        {
            Funds -= fundChange;
            FundsUpdated?.Invoke(Funds);
        }

        public bool HasFunds(int funds)
        {
            return Funds >= funds;
        }

        public bool HasGood(Good good)
        {
            return _goods.ContainsKey(good);
        }

        public bool HasGood(Good good, int amount)
        {
            return _goods.ContainsKey(good) && _goods[good] <= amount;
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

                var tier = GoodsInfoManager.Value.ConfigData[good].Tier;
                result[tier]++;
            }

            return result;
        }
    }
}