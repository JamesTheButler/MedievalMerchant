using System;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;

namespace Features.Goods.Selector
{
    public sealed class ComplexGoodSelector : IGoodSelector
    {
        private readonly Lazy<GoodsResources> _goodResources = new(() => ResourceManager.Instance.GoodsResources);

        private readonly Tier? _tier;
        private readonly Regions _regions;

        public ComplexGoodSelector(Tier? tier, Regions regions)
        {
            _tier = tier;
            _regions = regions;
        }

        public bool Matches(Good good)
        {
            var configData = _goodResources.Value.ConfigData[good];
            return (_tier == null || _tier == configData.Tier) && _regions.Intersects(configData.Regions);
        }
    }
}