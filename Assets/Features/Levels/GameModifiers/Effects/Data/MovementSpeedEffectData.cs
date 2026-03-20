using System;
using Common.UI.Utility;
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
                var style = SpeedBoostPercent.GetNumberStyle();
                return Loc.MovementSpeedEffect(SpeedBoostPercent).WithStyle(style);
            }
        }
    }
}