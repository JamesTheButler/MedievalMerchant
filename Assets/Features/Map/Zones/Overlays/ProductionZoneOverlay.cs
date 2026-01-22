using System;
using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Map.Zones.Overlays
{
    public sealed class ProductionZoneOverlay : MonoBehaviour, IOpenClosable
    {
        public event Action Opened;
        public event Action Closed;

        [SerializeField, Required]
        private TMP_Text regionNameText;

        [SerializeField, Required]
        private Image regionIcon;

        [SerializeField, Required]
        private ProductionZoneOverlayGoods goodsPrefab;

        [SerializeField, Required]
        private Transform zoneGoodContainer;

        [SerializeField, Required]
        private SimpleTooltipHandler regionIconTooltip;

        private readonly Bindings _bindings = new();

        private ProductionZone _productionZone;

        public void SetUp(ProductionZone zone)
        {
            _productionZone = zone;

            var regionConfig = ResourceManager.Instance.RegionResources;
            var regionData = regionConfig.Data[zone.Region];
            regionNameText.text = regionData.Name;
            regionIcon.sprite = regionData.Icon;
            regionIconTooltip.SetData($"Region: {regionData.Name}");

            var recipeResources = ResourceManager.Instance.RecipeResources;
            foreach (var good in zone.AvailableGoods)
            {
                var tier2Good = recipeResources.GetTier2RecipeForComponent(good).Result;
                AddGood(good, tier2Good);
            }
        }

        public void Open()
        {
            gameObject.SetActive(true);
            RefreshPosition();
        }

        public void Close()
        {
            _bindings.UnbindAll();
            gameObject.SetActive(false);
        }

        public void RefreshPosition()
        {
            var screenPosition = Camera.main!.WorldToScreenPoint(_productionZone.Center.FromXY());
            gameObject.transform.position = screenPosition;
        }

        private void AddGood(Good tier1, Good tier2)
        {
            var zoneGood = Instantiate(goodsPrefab, zoneGoodContainer);
            zoneGood.SetUp(tier1, tier2);
        }
    }
}