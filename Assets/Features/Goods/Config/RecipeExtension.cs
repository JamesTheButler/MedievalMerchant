using System.Linq;
using Common.Types;

namespace Features.Goods.Config
{
    public static class RecipeExtension
    {
        public static Good FirstOther(this RecipeConfigData recipe, Good good)
        {
            return recipe.Goods.First(g => g != good);
        }
    }
}