using System;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;
using Features.Localization.Data;

namespace Features.Goods.Selector
{
    public sealed class ComplexGoodSelector : IGoodSelector
    {
        private readonly Lazy<GoodResources> _goodResources = new(() => ResourceManager.Instance.GoodResources);

        private readonly Lazy<GoodLocalizationResources> _loc = new(() =>
            ResourceManager.Instance.LocalizationResources.Goods);

        private readonly Tier? _selectedTier;
        private readonly Regions _selectedRegions;

        public ComplexGoodSelector(Tier? selectedTier = null, Regions selectedRegions = Regions.All)
        {
            _selectedTier = selectedTier;
            _selectedRegions = selectedRegions;
        }

        public bool Matches(Good good)
        {
            var configData = _goodResources.Value.ResourceData[good];
            // HACK: _selectedTier is set from inspector 
            return (_selectedTier == null || _selectedTier == 0 || _selectedTier == configData.Tier) &&
                   _selectedRegions.Intersects(configData.Regions);
        }

        public string ToDisplayString()
        {
            var anyTier = _selectedTier is null or 0;
            var allRegions = (_selectedRegions & Regions.All) == Regions.All;

            return (anyTier, allRegions) switch
            {
                // for all goods from all regions
                (true, true) => _loc.Value.ComplexGoods_AnyTierAllRegions,
                // for all goods from {region list}
                (true, false) => _loc.Value.ComplexGoods_AnyTierSpecificRegions(_selectedRegions),
                // for tier X goods from all regions
                (false, true) => _loc.Value.ComplexGoods_SpecificTierAllRegions(_selectedTier!.Value),
                // for tier X goods from {region list}
                (false, false) => _loc.Value.ComplexGoods_SpecificTierSpecificRegions(
                    _selectedTier!.Value,
                    _selectedRegions),
            };
        }
    }
}