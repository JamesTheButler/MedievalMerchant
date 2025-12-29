using System;
using System.Collections.Generic;
using Common.Types;
using Common.Utility;
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
        private List<Good> goods;

        [SerializeField]
        private Regions regions = Regions.All;

        [SerializeField]
        private Tier tier;

        [CanBeNull]
        private IGoodSelector _selector;

        public IGoodSelector Selector => _selector ??= GetSelector();

        private IGoodSelector GetSelector()
        {
            if (applyToAll)
                return IGoodSelector.All;

            if (goods.Count == 1)
                return new SingleGoodSelector(goods[0]);

            if (goods.Count > 1)
                return new SpecificGoodsSelector(goods.ToArray());

            if (tier == 0 && regions == Regions.All && goods.IsEmpty())
                return IGoodSelector.All;

            return new ComplexGoodSelector(tier, regions);
        }
    }
}