using System.Collections.Generic;
using System.Linq;
using Common;
using Data.Configuration;
using UnityEngine;

namespace Data.Goods.Recipes.Config
{
    [CreateAssetMenu(fileName = nameof(RecipeConfig), menuName = AssetMenu.ConfigDataFolder + nameof(RecipeConfig))]
    public sealed class RecipeConfig : ScriptableObject
    {
        [SerializeField]
        private List<Tier2Recipe> tier2Recipes;

        [SerializeField]
        private List<Tier3Recipe> tier3Recipes;

        private readonly Dictionary<Good, Recipe> _recipes = new();

        private void Awake()
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

        public Tier2Recipe GetTier2Recipe(Good tier1Good)
        {
            return tier2Recipes.FirstOrDefault(recipe => recipe.Component == tier1Good);
        }

        public Tier3Recipe GetTier3Recipe(Good tier3Good)
        {
            return tier3Recipes.FirstOrDefault(recipe => recipe.Result == tier3Good);
        }

        public Recipe GetRecipe(Good good)
        {
            if (_recipes.TryGetValue(good, out var recipe)) return recipe;

            return new Recipe(good);
        }
    }
}