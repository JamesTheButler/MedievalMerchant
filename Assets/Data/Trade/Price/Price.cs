using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data.Trade.Price
{
    public sealed record Price
    {
        public float BasePrice { get; }
        public IEnumerable<PriceModifier> Modifiers { get; }
        public int FinalPrice { get; }

        public Price(float basePrice, IEnumerable<PriceModifier> modifiers)
        {
            BasePrice = basePrice;
            Modifiers = modifiers;
            FinalPrice = Mathf.FloorToInt(BasePrice * (1 + Modifiers.Sum(modifier => modifier.Value)));
        }
    }
}