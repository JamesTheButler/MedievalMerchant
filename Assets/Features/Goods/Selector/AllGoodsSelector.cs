using System;
using Common.Infrastructure;
using Common.Types;
using Features.Localization.Data;

namespace Features.Goods.Selector
{
    public sealed class AllGoodsSelector : IGoodSelector
    {
        private readonly Lazy<GoodLocalizationResources> _loc = new(() =>
            ResourceManager.Instance.LocalizationResources.Goods);

        public bool Matches(Good good)
        {
            return true;
        }

        public string ToDisplayString()
        {
            return _loc.Value.AllGoods;
        }
    }
}