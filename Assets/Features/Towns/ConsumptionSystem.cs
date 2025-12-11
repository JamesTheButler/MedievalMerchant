using System.Collections.Generic;
using Common.Types;
using Features.Ticking;
using Infrastructure;
using UnityEngine;

namespace Features.Towns
{
    public sealed class ConsumptionSystem : ISystem
    {
        private readonly Town _town;
        private readonly TickingService _tickingService;
        private readonly Dictionary<Good, IntBasedTicker> _consumptionTickers = new();
        
        private readonly Inventory.Inventory _townInventory;

        public ConsumptionSystem(Town town)
        {
            _town = town;
            _townInventory = _town.Inventory;
            // TODO MED-55
            //  _town.ProductionManager.ProducerAdded => might need to remove the ticker
        }

        private void OnGoodAdded(Good good)
        {
            // something weird happened in this case
            if (_consumptionTickers.ContainsKey(good))
                return;

            // don't consume goods that are produced
            if (_town.ProductionManager.IsProduced(good))
                return;

            // TODO: towns need to keep a list of observable<floats> for all the consumption rates => they should be modifiable
            var consumptionTicker = new IntBasedTicker(amount => OnConsumptionTick(good, amount), _town.ConsumptionRate);
            _tickingService.RegisterTicker(consumptionTicker);
            _consumptionTickers.Add(good, consumptionTicker);
        }

        private void OnConsumptionTick(Good good, int consumedAmount)
        {
            _town.Inventory.RemoveGood(good, consumedAmount);
        }

        private void OnGoodRemoved(Good good)
        {
            var ticker = _consumptionTickers[good];
            if (ticker == null)
            {
                Debug.LogWarning($"Could not find consumption ticker for good '{good}'");
                return;
            }
            _tickingService.UnregisterTicker(ticker);
            _consumptionTickers.Remove(good);
        }

        public void Initialize()
        {
            // just for safety, clear up anything that might be remaining
            ClearTickers();
            _townInventory.GoodAdded += OnGoodAdded;
            _townInventory.GoodRemoved += OnGoodRemoved;
        }

        public void CleanUp()
        {
            ClearTickers();
            _townInventory.GoodAdded -= OnGoodAdded;
            _townInventory.GoodRemoved -= OnGoodRemoved;
        }

        private void ClearTickers()
        {
            foreach (var ticker in _consumptionTickers.Values)
            {
                _tickingService.UnregisterTicker(ticker);
            }

            _consumptionTickers.Clear();
        }
    }
}