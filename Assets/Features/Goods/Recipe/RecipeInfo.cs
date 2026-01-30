using System;
using Common.Types;

namespace Features.Goods.Recipe
{
    public record RecipeInfo(Good Result, Good[] Components)
    {
        public RecipeInfo(Good Result) : this(Result, Array.Empty<Good>())
        {
        }
    }
}