using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Modifiable;
using Common.Types;
using Features.Goods;
using Features.Goods.Config;
using Features.Towns.Production.Config;
using UnityEngine;

namespace Features.Towns.Production.Logic
{
    public sealed class Producer
    {
        public ModifiableVariable ProductionRate { get; }
        public Good ProducedGood { get; }
        public Tier Tier { get; }

        private readonly Town _town;
        private readonly Recipe _recipe;

        private readonly GoodsConfig _goodsConfig;
        private readonly ProducerConfig _producerConfig;

        public Producer(Good producedGood, Town town)
        {
            _town = town;
            ProducedGood = producedGood;

            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
            _producerConfig = ConfigurationManager.Instance.ProducerConfig;
            var recipeConfig = ConfigurationManager.Instance.RecipeConfig;

            Tier = _goodsConfig.ConfigData[producedGood].Tier;

            var baseModifier = new BaseProductionValue(producedGood);
            ProductionRate = new ModifiableVariable("Production Rate", true, baseModifier);

            _recipe = recipeConfig.GetRecipe(producedGood);
        }

        public void Produce()
        {
            if (!CanProduce()) return;

            var limit = GetProductionLimit(_town.Tier.Value, ProducedGood);
            var currentInventoryAmount = _town.Inventory.Goods.GetValueOrDefault(ProducedGood, 0);
            var cappedAmount = Mathf.Min(ProductionRate, Mathf.Max(0, limit - currentInventoryAmount));
            _town.Inventory.AddGood(ProducedGood, (int)cappedAmount);

            foreach (var component in _recipe.Components)
            {
                _town.Inventory.RemoveGood(component, (int)_producerConfig.ConsumptionRate);
            }
        }

        private bool CanProduce()
        {
            return _recipe.Components?.All(component =>
                _town.Inventory.HasGood(component, (int)_producerConfig.ConsumptionRate)) ?? false;
        }

        private int GetProductionLimit(Tier townTier, Good good)
        {
            var goodTier = _goodsConfig.ConfigData[good].Tier;
            var limit = _producerConfig.GetLimit(townTier, goodTier);
            if (limit != null)
                return limit.Value;

            Debug.LogError($"No production limit is set for town {townTier} and good {goodTier}.");
            return 0;
        }
    }
}