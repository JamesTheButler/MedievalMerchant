using System;
using UnityEngine;

namespace Data.Configuration
{
    [Serializable]
    public sealed class Tier3Recipe
    {
        [field: SerializeField]
        public Good Tier3Good { get; private set; }
        
        [field: SerializeField]
        public Good Good1 { get; private set; }

        [field: SerializeField]
        public Good Good2 { get; private set; }

        public bool Contains(Good good)
        {
            return Good1 == good || Good2 == good;
        }
    }
}