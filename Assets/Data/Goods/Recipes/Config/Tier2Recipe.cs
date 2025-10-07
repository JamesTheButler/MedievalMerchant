using System;
using UnityEngine;

namespace Data.Goods.Recipes.Config
{
    [Serializable]
    public sealed class Tier2Recipe
    {
        [field: SerializeField]
        public Good Component { get; private set; }
        
        [field: SerializeField]
        public Good Result { get; private set; }
    }
}