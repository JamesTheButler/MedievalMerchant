using System;
using System.Collections.Generic;
using Common.Types;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Goods.Selector
{
    [Serializable]
    public sealed class GoodSelectorData
    {
        [SerializeField]
        private bool applyToAll;

        [SerializeField]
        private bool limitTier;

        [SerializeField]
        private List<Good> goods;

        [SerializeField]
        private Regions regions = Regions.All;

        [SerializeField, ShowIf(nameof(limitTier))]
        private Tier tier;

        [CanBeNull]
        private IGoodSelector _selector;

        public IGoodSelector Selector => _selector ??= GetSelector();

        private IGoodSelector GetSelector()
        {
            if (applyToAll) return IGoodSelector.All;
            if (goods.Count == 1) return new SingleGoodSelector(goods[0]);
            if (goods.Count > 1) return new SpecificGoodsSelector(goods.ToArray());

            return new ComplexGoodSelector(limitTier ? tier : null, regions);
        }
    }
}