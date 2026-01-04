using System;
using System.Linq;
using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;

namespace Features.Goods.Selector
{
    public sealed class ComplexGoodSelector : IGoodSelector
    {
        private readonly Lazy<GoodsResources> _goodResources = new(() => ResourceManager.Instance.GoodsResources);
        private readonly Lazy<RegionResources> _regionResources = new(() => ResourceManager.Instance.RegionResources);

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
            return (_selectedTier == null || _selectedTier == 0|| _selectedTier == configData.Tier) &&
                   _selectedRegions.Intersects(configData.Regions);
        }

        public string ToDisplayString()
        {
            var tierString = _selectedTier is null or 0 ? "all" : _selectedTier.Value.ToDisplayString();

            string regionsString;
            if ((_selectedRegions & Regions.All) == Regions.All)
            {
                regionsString = "all regions";
            }
            else
            {
                regionsString = EnumExtensions.Enumerate<Region>()
                    .Where(region => _selectedRegions.Contains(region))
                    .Select(region => _regionResources.Value.Data[region].Name)
                    .JoinWithAnd();
            }

            // e.g. "for Tier1 goods from Oceans, Fields and Mountains"
            // e.g. "for all goods from Oceans"
            // e.g. "for Tier2 goods from all regions"
            return $"for {tierString} goods from {regionsString}";
        }
    }
}