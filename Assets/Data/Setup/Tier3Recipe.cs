using System;
using UnityEngine;

namespace Data.Setup
{
    [Serializable]
    public sealed class Tier3Recipe
    {
        [field: SerializeField] public Good Good1 { get; private set; }
        [field: SerializeField] public Good Good2 { get; private set; }
    }
}