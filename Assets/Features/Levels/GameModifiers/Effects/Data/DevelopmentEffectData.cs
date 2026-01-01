using System;
using Common.UI.Utility;
using Common.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [Serializable]
    public sealed class DevelopmentEffectData : EffectData
    {
        [field: SerializeField, Range(-1f, 2f)]
        public float DevelopmentBoostPercent { get; private set; }

        private string _description;

        public override string Description
        {
            get
            {
                var valueString = DevelopmentBoostPercent.ToPercentString(true);
                var style = DevelopmentBoostPercent.GetNumberStyle();
                return $"{valueString} development change rate".WithStyle(style);
            }
        }
    }
}