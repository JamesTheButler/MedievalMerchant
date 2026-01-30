using System;
using Common.Types;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Recipe
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

        public Good GetOtherComponent(Good component)
        {
            return Component1 == component ? Component2 : Component1;
        }
    }
}