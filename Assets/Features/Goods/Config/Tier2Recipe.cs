using System;
using Common.Types;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Config
{
    [Serializable]
    public sealed class Tier2Recipe
    {
        [field: SerializeField]
        public Good Result { get; private set; }

        [field: SerializeField, HorizontalLine]
        public Good Component { get; private set; }
    }
}