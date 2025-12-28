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

        private readonly GoodsResources _goodsResources;
        private readonly ProducerConfig _producerConfig;

        private readonly Dictionary<Good, ModifiableVariable> _ingredientConsumptionRates = new();

        public Producer(Good producedGood, Town town)
        {
            ProducedGood = producedGood;

            _goodsResources = ResourceManager.Instance.GoodsResources;
            _producerConfig = ConfigurationManager.Configurations.ProducerConfig;
            var recipeConfig = ResourceManager.Instance.RecipeResources;

            Tier = _goodsResources.ResourceData[producedGood].Tier;

            var baseProductionRate = new BaseProductionValue(producedGood);
            ProductionRate = new ModifiableVariable("Production Rate", true, baseProductionRate);
            recipeConfig.GetRecipe(producedGood);

            town.Tier.Observe(OnTownTierChanged);
        }

        private void OnTownTierChanged(Tier townTier)
        {
            var goodTier = _goodsResources.ResourceData[ProducedGood].Tier;
            var configLimit = _producerConfig.GetLimit(townTier, goodTier);
            ProductionLimit = configLimit ?? 0;
            
            if (configLimit == null)
            {
                Debug.LogError($"No production limit is set for town {townTier} and good {goodTier}.");
            }
        }
    }
}