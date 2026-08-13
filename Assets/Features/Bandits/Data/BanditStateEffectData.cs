using System;
using UnityEngine;

namespace Features.Bandits.Data
{
    /// <summary>
    /// Combat/movement percentage change applied while a bandit group is in a given behavior state.
    /// Follows the BasePercentageModifier convention: 0 .. no change, 1 .. +100%, -1 .. -100%.
    /// </summary>
    [Serializable]
    public sealed class BanditStateEffectData
    {
        [field: SerializeField, Range(-1f, 1f)]
        public float StrengthEffect { get; private set; }

        [field: SerializeField, Range(-1f, 1f)]
        public float MovementSpeedEffect { get; private set; }
    }
}
