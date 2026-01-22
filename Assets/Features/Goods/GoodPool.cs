using System.Collections.Generic;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Map;
using Features.Map.Zones;

namespace Features.Goods
{
    public sealed class GoodPool
    {
        private readonly HashSet<Good> _availableGoods;
        private readonly HashSet<Good> _tier1Goods;
        private readonly HashSet<Good> _tier2Goods;
        private readonly HashSet<Good> _tier3Goods;

        public IReadOnlyCollection<Good> AllAvailableGoods => _availableGoods;
        public IReadOnlyCollection<Good> Tier1Goods => _tier1Goods;
        public IReadOnlyCollection<Good> Tier2Goods => _tier2Goods;
        public IReadOnlyCollection<Good> Tier3Goods => _tier3Goods;

        public GoodPool(ProductionZone[] zones)
        {
            var recipeResources = ResourceManager.Instance.RecipeResources;

            _tier1Goods = new HashSet<Good>();
            _tier2Goods = new HashSet<Good>();
            _tier3Goods = new HashSet<Good>();

            foreach (var zone in zones)
            {
                foreach (var good in zone.AvailableGoods)
                {
                    if (!_tier1Goods.Add(good))
                        continue;

                    var tier2Good = recipeResources.GetTier2RecipeForComponent(good).Result;
                    _tier2Goods.Add(tier2Good);
                }
            }

            foreach (var recipe in recipeResources.Tier3Recipes)
            {
                if (_tier2Goods.Contains(recipe.Component1) && _tier2Goods.Contains(recipe.Component2))
                {
                    _tier3Goods.Add(recipe.Result);
                }
            }

            _availableGoods = new HashSet<Good>
            {
                _tier1Goods,
                _tier2Goods,
                _tier3Goods
            };
        }
    }
}