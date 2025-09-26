using System.Collections.Generic;
using System.Linq;
using AYellowpaper.SerializedCollections;
using UnityEngine;

namespace Data.Configuration
{
    [CreateAssetMenu(fileName = nameof(RecipeConfig), menuName = AssetMenu.ConfigDataFolder + nameof(RecipeConfig))]
    public sealed class RecipeConfig : ScriptableObject
    {
        [field: SerializeField, SerializedDictionary("Tier 1 Good", "Tier 2 Good")]
        public SerializedDictionary<Good, Good> Tier2Recipes { get; private set; }

        [field: SerializeField, SerializedDictionary("Tier 3 Good", "Components")]
        public SerializedDictionary<Good, Tier3Recipe> Tier3Recipes { get; private set; }

        [SerializeField]
        private List<Tier3Recipe> tier3RecipeList;

        public List<Tier3Recipe> GetRecipes(Good tier2Good)
        {
            return tier3RecipeList.Where(recipe => recipe.Contains(tier2Good)).ToList();
        }

        public Tier3Recipe GetRecipe(Good tier3Good)
        {
            return tier3RecipeList.First(recipe => recipe.Tier3Good == tier3Good);
        }

        public Good? TryGetRecipe(Good tier2Good1, Good tier2Good2)
        {
            if(!Tier3Recipes.Any(pair => pair.Value.Contains(tier2Good1) && pair.Value.Contains(tier2Good2)))
                return null;
            
            return Tier3Recipes
                .FirstOrDefault(pair => pair.Value.Contains(tier2Good1) && pair.Value.Contains(tier2Good2)).Key;
        }
    }
}