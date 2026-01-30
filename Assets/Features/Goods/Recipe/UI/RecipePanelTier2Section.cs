using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;
using Features.Goods.Recipe.Data;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Recipe.UI
{
    public sealed class RecipePanelTier2Section : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject rootObject, groupContainer;

        [SerializeField, Required]
        private RecipePanelTier2RegionGroup groupPrefab;

        private GoodResources _goodResources;
        private RecipeResources _recipeResources;

        public void Initialize()
        {
            _goodResources = ResourceManager.Instance.GoodResources;
            _recipeResources = ResourceManager.Instance.RecipeResources;

            groupContainer.DestroyChildren();

            var regions = new List<Region>
            {
                Region.Forest,
                Region.Ocean,
                Region.Mountains,
                Region.Fields,
            };

            foreach (var region in regions)
            {
                var goods = _recipeResources.Tier2Recipes
                    .Where(recipe => _goodResources.ResourceData[recipe.Component].Regions.Contains(region))
                    .ToArray();
                var regionGroup = Instantiate(groupPrefab, groupContainer.transform);
                regionGroup.SetUp(region, goods);
            }
        }
    }
}