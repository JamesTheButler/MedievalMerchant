using System;
using Common.Config;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Common.UI.Popups
{
    public sealed class ProductionZonePopup : Popup
    {
        [SerializeField, Required]
        private TMP_Text regionName;

        [SerializeField, Required]
        private Image regionIcon;

        [SerializeField, Required]
        private GameObject zoneGoodPrefab;

        [SerializeField, Required]
        private GameObject zoneGoodContainer;

        [SerializeField, Required]
        private SimpleTooltipHandler regionIconTooltip;

        private readonly Lazy<RegionResources> _regionConfig = new(() => ResourceManager.Instance.RegionResources);

        public void Reset()
        {
            zoneGoodContainer.DestroyChildren<ZoneGood>();
        }

        public void SetRegion(Region region)
        {
            var regionData = _regionConfig.Value.Data[region];
            regionName.text = regionData.Name;
            regionIcon.sprite = regionData.Icon;
            regionIconTooltip.SetData($"Region: {regionData.Name}");
        }

        public void AddGood(Good tier1, Good tier2)
        {
            var goodObject = Instantiate(zoneGoodPrefab, zoneGoodContainer.transform);
            var zoneGood = goodObject.GetComponent<ZoneGood>();
            zoneGood.SetUp(tier1, tier2);
        }
    }
}