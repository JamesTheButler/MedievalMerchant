using Common.Infrastructure;
using Common.Utility;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Recipe.UI
{
    public sealed class RecipePanelTier3Section : MonoBehaviour
    {
        [SerializeField, Required]
        private RecipePanelTier3Group tier3GroupPrefab;

        [SerializeField, Required]
        private RectTransform container;

        public void Initialize()
        {
            container.DestroyChildren();

            var recipeResources = ResourceManager.Instance.RecipeResources;
            foreach (var recipe in recipeResources.Tier3Recipes)
            {
                var group = Instantiate(tier3GroupPrefab, container);
                var t1Component1 = recipeResources.GetTier2RecipeForResult(recipe.Component1).Component;
                var t1Component2 = recipeResources.GetTier2RecipeForResult(recipe.Component2).Component;
                group.Setup(t1Component1, recipe.Component1, t1Component2, recipe.Component2, recipe.Result);
            }
        }
    }
}