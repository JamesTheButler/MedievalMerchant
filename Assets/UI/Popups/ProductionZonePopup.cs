using System;
using Common;
using Common.Config;
using Common.Types;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
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

        private readonly Lazy<RegionConfig> _regionConfig = new(() => ConfigurationManager.Instance.RegionConfig);

        public void Reset()
        {
            zoneGoodContainer.DestroyChildren<ZoneGood>();
        }

        public void SetRegion(Region region)
        {
            regionName.text = _regionConfig.Value.Data[region].Name;
            regionIcon.sprite = _regionConfig.Value.Data[region].Icon;
        }

        public void AddGood(Good tier1, Good tier2)
        {
            var goodObject = Instantiate(zoneGoodPrefab, zoneGoodContainer.transform);
            var zoneGood = goodObject.GetComponent<ZoneGood>();
            zoneGood.SetUp(tier1, tier2);
        }
    }
}