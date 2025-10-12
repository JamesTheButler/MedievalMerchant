using System;
using Common.Types;

namespace Features.Goods
{
    public record Recipe(Good Result, Good[] Components)
    {
        public Recipe(Good Result) : this(Result, Array.Empty<Good>())
        {
        }
    }
}