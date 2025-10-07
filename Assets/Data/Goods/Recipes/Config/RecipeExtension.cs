using System.Linq;

namespace Data.Goods.Recipes.Config
{
    public static class RecipeExtension
    {
        public static Good FirstOther(this RecipeConfigData recipe, Good good)
        {
            return recipe.Goods.First(g => g != good);
        }
    }
}