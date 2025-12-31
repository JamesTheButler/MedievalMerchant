using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Features.Ticking.Logic;
using UnityEngine;

namespace Features.Towns.Production.Logic
{
    public sealed class TownProductionSystem : ISystem
    {
        private readonly Town _town;
        private readonly Dictionary<Producer, IntBasedTicker> _productionTickers = new();
        private readonly Dictionary<(Producer, Good), IntBasedTicker> _ingredientTickers = new();

        private TickingService _tickingService;

        public TownProductionSystem(Town town)
        {
            _town = town;
        }

        public void Initialize()
        {
            _tickingService = GameplayContext.Instance.Services.TickingService;
            _town.ProductionManager.ProductionAdded += OnProducerAdded;
            foreach (var producer in _town.ProductionManager.AllProducers)
            {
                OnProducerAdded(producer);
            }

        }

        public void CleanUp()
        {
            _town.ProductionManager.ProductionAdded -= OnProducerAdded;
            ClearTickers();
        }

        private void OnProducerAdded(Producer producer)
        {
            if (_productionTickers.ContainsKey(producer))
            {
                Debug.LogWarning($"A production ticker has already been added for '{producer.ProducedGood}'");
                return;
            }

            RegisterProductionTicker(producer);
            RegisterIngredientTickers(producer);
        }

        private void RegisterProductionTicker(Producer producer)
        {
            var productionTicker = new IntBasedTicker(
                productionRate => OnProductionTick(producer, productionRate),
                producer.ProductionRate);
            // TODO this dangles refs!!!!!
            producer.ProductionRate.Observe(productionRate => OnProductionRateChanged(producer, productionRate));
            _tickingService.RegisterTicker(productionTicker);
            _productionTickers.Add(producer, productionTicker);
        }

        private void OnProductionTick(Producer producer, int productionRate)
        {
            if (!CanProduce(producer))
                return;

            var limit = producer.ProductionLimit;
            var currentAmount = _town.Inventory.Goods.GetValueOrDefault(producer.ProducedGood, 0);
            var cappedAmount = Mathf.Min(productionRate, Mathf.Max(0, limit - currentAmount));
            _town.Inventory.AddGood(producer.ProducedGood, cappedAmount);
        }

        private void RegisterIngredientTickers(Producer producer)
        {
            foreach (var (ingredient, consumptionRate) in producer.IngredientConsumptionRates)
            {
                var consumptionTicker = new IntBasedTicker(
                    rate => OnConsumptionTick(producer, ingredient, rate),
                    consumptionRate);

                // TODO this dangles refs!!!!!
                consumptionRate.Observe(newConsumptionRate =>
                    _ingredientTickers[(producer, ingredient)].ValueRatePerDay = newConsumptionRate);

                _tickingService.RegisterTicker(consumptionTicker);
                _ingredientTickers.Add((producer, ingredient), consumptionTicker);
            }
        }

        private void OnConsumptionTick(Producer producer, Good ingredient, int rate)
        {
            if (!CanProduce(producer))
                return;

            _town.Inventory.RemoveGood(ingredient, rate);
        }

        private void OnProductionRateChanged(Producer producer, float productionRate)
        {
            if (!_productionTickers.TryGetValue(producer, out var ticker))
                return;

            ticker.ValueRatePerDay = productionRate;
        }

        private void ClearTickers()
        {
            foreach (var ticker in _productionTickers.Values)
            {
                _tickingService.UnregisterTicker(ticker);
            }

            _productionTickers.Clear();
        }


        private bool CanProduce(Producer producer)
        {
            foreach (var (good, consumptionRate) in producer.IngredientConsumptionRates)
            {
                if (!_town.Inventory.HasGood(good, (int)consumptionRate.Value))
                    return false;
            }

            return true;
        }
    }
}