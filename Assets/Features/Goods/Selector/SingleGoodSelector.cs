using System;
using Common.Infrastructure;
using Common.Types;
using Features.Localization.Data;

namespace Features.Goods.Selector
{
    public sealed class SingleGoodSelector : IGoodSelector
    {
        private readonly Lazy<GoodLocalizationResources> _loc = new(() =>
            ResourceManager.Instance.LocalizationResources.Goods);

        private readonly Good _good;

        public SingleGoodSelector(Good good)
        {
            _good = good;
        }

        public bool Matches(Good good)
        {
            return _good == good;
        }

        public string ToDisplayString()
        {
            return _loc.Value.SingleGood(_good);
        }
    }
}