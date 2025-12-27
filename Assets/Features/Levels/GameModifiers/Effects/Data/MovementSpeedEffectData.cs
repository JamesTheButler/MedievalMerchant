using System;
using Common.UI.Utility;
using Common.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [Serializable]
    public sealed class MovementSpeedEffectData : EffectData
    {
        [field: SerializeField, Range(-1f, 2f)]
        public float SpeedBoostPercent { get; private set; }

        public override string Description
        {
            get
            {
                var valueString = SpeedBoostPercent.ToPercentString(true);
                var style = SpeedBoostPercent > 0 ? Style.Good : Style.Bad;
                return $"{valueString} caravan movement speed".WithStyle(style);
            }
        }
    }
}