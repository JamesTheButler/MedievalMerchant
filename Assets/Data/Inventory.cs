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
                _goods[good] = 0;
            }

            GoodUpdated?.Invoke(good, _goods[good]);
        }

        public int Get(Good good)
        {
            return _goods.GetValueOrDefault(good, 0);
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