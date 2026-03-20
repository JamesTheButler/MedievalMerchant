using System;
using Common.UI.Utility;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [Serializable]
    public sealed class ReputationEffectData : EffectData
    {
        [field: SerializeField, Range(-1f, 2f)]
        public float ReputationBoostPercent { get; private set; }

        private string _description;

        public override string Description
        {
            get
            {
                var style = ReputationBoostPercent.GetNumberStyle();
                return Loc.ReputationEffect(ReputationBoostPercent).WithStyle(style);
            }
        }
    }
}