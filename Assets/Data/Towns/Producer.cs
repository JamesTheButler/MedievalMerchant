using System;
using System.Collections.Generic;
using Data.Trade;
using UnityEngine;

namespace Data.Towns
{
    public sealed class Producer
    {
        public event Action<Good> GoodAdded;

        private readonly Inventory _inventory;
        public IEnumerable<Good> ProducedGoods => _producedGoods.Keys;

        // TODO: config file
        private const int BaseProduction = 4;

        private readonly Dictionary<Good, int> _producedGoods = new();
        private float _multiplier = 1f;

        public Producer(Inventory inventory)
        {
            _inventory = inventory;
        }

        public void AddProduction(Good good, int limit)
        {
            if (!_producedGoods.ContainsKey(good))
            {
                GoodAdded?.Invoke(good);
            }

            _producedGoods[good] = limit;
        }

        public void Produce()
        {
            var amount = (int)(BaseProduction * _multiplier);
            foreach (var (good, limit) in _producedGoods)
            {
                var cappedAmount = Mathf.Min(amount, Mathf.Max(0, limit - _inventory.Goods.GetValueOrDefault(good, 0)));
                _inventory.AddGood(good, cappedAmount);
            }
        }

        public void SetProductionMultiplier(float multiplier)
        {
            _multiplier = multiplier;
        }
    }
}