using System;

namespace Data.Goods.Recipes
{
    public record Recipe(Good Result, Good[] Components)
    {
        public Recipe(Good Result) : this(Result, Array.Empty<Good>())
        {
        }
    }
}