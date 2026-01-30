using System.Linq;
using Common.Types;
using Features.Goods.Recipe.Data;

namespace Features.Goods.Recipe
{
    public static class RecipeExtension
    {
        public static Good FirstOther(this RecipeConfigData recipe, Good good)
        {
            return recipe.Goods.First(g => g != good);
        }
    }
}