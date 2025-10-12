using System;
using Common.Types;
using UnityEngine;

namespace Features.Goods.Config
{
    [Serializable]
    public sealed class RecipeConfigData
    {
        [field: SerializeField]
        public Good[] Goods { get; private set; }
    }
}