using System;
using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Features.Localization.Data;

namespace Features.Goods.Selector
{
    public sealed class SpecificGoodsSelector : IGoodSelector
    {
        private readonly Lazy<GoodLocalizationResources> _loc = new(() =>
            ResourceManager.Instance.LocalizationResources.Goods);

        private readonly Good[] _goods;

        public SpecificGoodsSelector(Good[] goods)
        {
            _goods = goods;
        }

        public bool Matches(Good good)
        {
            return _goods.Contains(good);
        }

        public string ToDisplayString()
        {
            return _loc.Value.SpecificGoods(_goods);
        }
    }
}