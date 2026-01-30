using System;
using Common.Types;
using UnityEngine;

namespace Features.Goods.Recipe.Data
{
    [Serializable]
    public sealed class RecipeConfigData
    {
        [field: SerializeField]
        public Good[] Goods { get; private set; }
    }
}