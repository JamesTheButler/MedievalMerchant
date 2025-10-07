using System;
using UnityEngine;

namespace Data.Goods.Recipes.Config
{
    [Serializable]
    public sealed class RecipeConfigData
    {
        [field: SerializeField]
        public Good[] Goods { get; private set; }
    }
}