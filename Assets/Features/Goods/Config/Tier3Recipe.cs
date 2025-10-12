using System;
using Common.Types;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Config
{
    [Serializable]
    public sealed class Tier3Recipe
    {
        [field: SerializeField]
        public Good Result { get; private set; }

        [field: SerializeField, HorizontalLine]
        public Good Component1 { get; private set; }

        [field: SerializeField]
        public Good Component2 { get; private set; }

        public Good GetOther(Good good)
        {
            return Component1 == good ? Component2 : Component1;
        }
    }
}