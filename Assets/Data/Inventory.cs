using System;
using System.Collections.Generic;

namespace Data
{
    public class Inventory
    {
        public event Action<int> FundsUpdated;
        public event Action<Good, int> GoodUpdated;

        public int Funds { get; private set; }

        public IReadOnlyDictionary<Good, int> Goods => _goods;

        private readonly Dictionary<Good, int> _goods = new();

        public void AddFunds(int count)
        {
            Funds += count;
            FundsUpdated?.Invoke(count);
        }

        public void RemoveFunds(int count)
        {
            Funds -= count;
            FundsUpdated?.Invoke(count);
        }

        public bool HasFunds(int funds)
        {
            return Funds >= funds;
        }

        public void AddGood(Good good, int amount)
        {
            _goods.TryAdd(good, 0);
            _goods[good] += amount;
            GoodUpdated?.Invoke(good, _goods[good]);
        }

        public bool HasGood(Good good, int amount)
        {
            if (!_goods.TryGetValue(good, out var currentAmount)) return false;

            return currentAmount >= amount;
        }

        public bool RemoveGood(Good good, int amount)
        {
            if (!HasGood(good, amount)) return false;

            _goods[good] -= amount;

            if (_goods[good] <= 0)
            {
                _goods[good] = 0;
            }

            GoodUpdated?.Invoke(good, _goods[good]);
            return true;
        }

        public bool SellTo(Inventory other, Good good, int amount, int totalPrice)
        {
            // we don't have enough of this good
            if (!HasGood(good, amount)) return false;
            // other inventory does not have enough money
            if (!other.HasFunds(totalPrice)) return false;

            // sell
            RemoveGood(good, amount);
            AddFunds(totalPrice);
            // buy
            other.AddGood(good, amount);
            other.RemoveFunds(totalPrice);

            return true;
        }
    }
}