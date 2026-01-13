using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Features.Goods.Config;
using Features.Towns.Production.Config;
using UnityEngine;

namespace Features.Towns.Production.Logic
{
    public sealed class Producer
    {
        public Good ProducedGood { get; }
        public Tier Tier { get; }
        public int ProductionLimit { get; private set; }
        public ModifiableVariable ProductionRate { get; }

        public IReadOnlyDictionary<Good, ModifiableVariable> IngredientConsumptionRates => _ingredientConsumptionRates;

        private readonly GoodResources _goodResources;
        private readonly ProducerConfig _producerConfig;

        private readonly Dictionary<Good, ModifiableVariable> _ingredientConsumptionRates = new();

        public Producer(Good producedGood, Town town)
        {
            ProducedGood = producedGood;

            _goodResources = ResourceManager.Instance.GoodResources;
            _producerConfig = ConfigurationManager.Configurations.ProducerConfig;
            var recipeConfig = ResourceManager.Instance.RecipeResources;

            Tier = _goodResources.ResourceData[producedGood].Tier;

            var baseProductionRate = new BaseProductionValue(producedGood);
            ProductionRate = new ModifiableVariable("Production Rate", true, baseProductionRate);
            var recipe = recipeConfig.GetRecipe(producedGood);

            var baseConsumptionRate = new BaseConsumptionValue();
            foreach (var ingredient in recipe.Components)
            {
                var consumptionModVar = new ModifiableVariable("Consumption Rate", false, baseConsumptionRate);
                _ingredientConsumptionRates.Add(ingredient, consumptionModVar);
            }

            town.Tier.Observe(OnTownTierChanged);
        }

        private void OnTownTierChanged(Tier townTier)
        {
            var goodTier = _goodResources.ResourceData[ProducedGood].Tier;
            var configLimit = _producerConfig.GetLimit(townTier, goodTier);
            ProductionLimit = configLimit ?? 0;

            if (configLimit == null)
            {
                Debug.LogError($"No production limit is set for town {townTier} and good {goodTier}.");
            }
        }
    }
}