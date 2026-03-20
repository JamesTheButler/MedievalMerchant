using System;
using System.Collections.Generic;
using System.Linq;
using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;
using Features.Goods.Config;
using UnityEngine;
using UnityEngine.Localization;

namespace Features.Localization.Data
{
    [Serializable]
    public sealed class GoodLocalizationResources
    {
        private readonly Lazy<RegionResources> _regionResources = new(() => ResourceManager.Instance.RegionResources);
        private readonly Lazy<GoodResources> _goodResources = new(() => ResourceManager.Instance.GoodResources);

        private RegionResources RegionResources => _regionResources.Value;
        private GoodResources GoodResources => _goodResources.Value;

        [SerializeField]
        private LocalizedString allGoods,
            singleGoods,
            specificGoods,
            anyTierAllRegions,
            anyTierSpecificRegions,
            specificTierAllRegions,
            specificTierSpecificRegions;

        public string AllGoods => allGoods.GetLocalizedString();

        public string SingleGood(Good good)
        {
            var args = new { GoodName = GoodResources.ResourceData[good].GoodName };
            return singleGoods.GetLocalizedString(args);
        }

        public string SpecificGoods(Good[] goods)
        {
            var goodNames = goods.Select(good => GoodResources.ResourceData[good].GoodName);
            var args = new
            {
                GoodsList = JoinWithAnd(goodNames),
            };

            return specificGoods.GetLocalizedString(args);
        }

        // ReSharper disable once InconsistentNaming
        public string ComplexGoods_AnyTierAllRegions => anyTierAllRegions.GetLocalizedString();

        public string ComplexGoods_AnyTierSpecificRegions(Regions regions)
        {
            var args = new { RegionList = RegionListString(regions) };
            return anyTierSpecificRegions.GetLocalizedString(args);
        }

        public string ComplexGoods_SpecificTierAllRegions(Tier tier)
        {
            var args = new { TierRoman = tier.ToRomanNumeral() };
            return specificTierAllRegions.GetLocalizedString(args);
        }

        public string ComplexGoods_SpecificTierSpecificRegions(Tier tier, Regions regions)
        {
            var args = new
            {
                TierRoman = tier.ToRomanNumeral(),
                RegionList = RegionListString(regions),
            };
            return specificTierSpecificRegions.GetLocalizedString(args);
        }

        private string RegionListString(Regions regions)
        {
            var regionNames = EnumExtensions.Enumerate<Region>()
                .Where(region => regions.Contains(region))
                .Select(region => RegionResources.Data[region].Name);

            return JoinWithAnd(regionNames);
        }

        private string JoinWithAnd(IEnumerable<string> strings)
        {
            var array = strings.ToArray();
            var count = array.Length;

            return count switch
            {
                0 => string.Empty,
                1 => array[0],
                _ => string.Join(", ", array[..^1]) + $" {ResourceManager.Instance.LocalizationResources.And} " +
                     array[^1]
            };
        }
    }
}