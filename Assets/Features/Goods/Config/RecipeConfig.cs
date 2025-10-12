using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Types;
using UnityEngine;

namespace Features.Goods.Config
{
    [CreateAssetMenu(fileName = nameof(RecipeConfig), menuName = AssetMenu.ConfigDataFolder + nameof(RecipeConfig))]
    public sealed class RecipeConfig : ScriptableObject
    {
        [SerializeField]
        private List<Tier2Recipe> tier2Recipes;

        [SerializeField]
        private List<Tier3Recipe> tier3Recipes;

        private readonly Dictionary<Good, Recipe> _recipes = new();

        public void Initialize()
        {
            _recipes.Clear();
            _recipes.AddValues(
                tier2Recipes.Select(recipeData => new Recipe(recipeData.Result, recipeData.Component.AsArray())),
                recipe => recipe.Result);

            _recipes.AddValues(
                tier3Recipes.Select(recipeData =>
                    new Recipe(recipeData.Result, new[] { recipeData.Component1, recipeData.Component2 })),
                recipe => recipe.Result);
        }

        public Tier2Recipe GetTier2RecipeForComponent(Good component)
        {
            return tier2Recipes.FirstOrDefault(recipe => recipe.Component == component);
        }

        public Tier2Recipe GetTier2RecipeForResult(Good result)
        {
            return tier2Recipes.FirstOrDefault(recipe => recipe.Result == result);
        }

        public Tier3Recipe[] GetTier3RecipeForComponent(Good component)
        {
            return tier3Recipes
                .Where(recipe => recipe.Component1 == component || recipe.Component2 == component)
                .ToArray();
        }

        public Tier3Recipe GetTier3RecipeForResult(Good result)
        {
            return tier3Recipes.FirstOrDefault(recipe => recipe.Result == result);
        }

        public Recipe GetRecipe(Good good)
        {
            if (_recipes.TryGetValue(good, out var recipe)) return recipe;

            return new Recipe(good);
        }
    }
}