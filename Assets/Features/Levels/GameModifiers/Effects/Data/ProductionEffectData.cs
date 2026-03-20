using System;
using Common.UI.Utility;
using Features.Goods.Selector;
using UnityEngine;

namespace Features.Levels.GameModifiers.Effects.Data
{
    [Serializable]
    public sealed class ProductionEffectData : EffectData
    {
        [field: SerializeField, Range(-1f, 2f)]
        public float ProductionBoostPercent { get; private set; }

        [field: SerializeField]
        public GoodSelectorData Selector { get; private set; }

        private string _description;

        public override string Description
        {
            get
            {
                var selectorString = Selector.Selector.ToDisplayString();
                var style = ProductionBoostPercent > 0 ? Style.Good : Style.Bad;
                return Loc.ProductionEffect(ProductionBoostPercent, selectorString).WithStyle(style);
            }
        }
    }
}