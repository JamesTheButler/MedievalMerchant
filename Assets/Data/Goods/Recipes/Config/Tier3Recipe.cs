using System;
using UnityEngine;

namespace Data.Goods.Recipes.Config
{
    [Serializable]
    public sealed class Tier3Recipe
    {
        [field: SerializeField]
        public Good Component1 { get; private set; }

        [field: SerializeField]
        public Good Component2 { get; private set; }

        [field: SerializeField]
        public Good Result { get; private set; }
    }
}