using Common.Infrastructure;
using Common.Utility;
using Features.Goods.Config;
using Features.Map;
using UnityEngine;

namespace Common.UI.Popups
{
    public sealed class ProductionZonePopupHandler : MonoBehaviour
    {
        [SerializeField]
        private ProductionZonePopup productionZonePopup;

        [SerializeField]
        private Grid grid;

        private RecipeResources _recipeResources;
        private ProductionZone _zone;

        private void Start()
        {
            _recipeResources = ResourceManager.Instance.RecipeResources;
            Unbind();
        }

        public void Bind(ProductionZone zone)
        {
            if (_zone == zone)
                return;

            if (zone == null)
            {
                Unbind();
                return;
            }

            _zone = zone;
            productionZonePopup.Reset();
            RefreshPosition();
            productionZonePopup.SetRegion(zone.Region);
            foreach (var tier1Good in zone.AvailableGoods)
            {
                var tier2Good = _recipeResources.GetTier2RecipeForComponent(tier1Good).Result;
                productionZonePopup.AddGood(tier1Good, tier2Good);
            }

            productionZonePopup.Show();
        }

        public void RefreshPosition()
        {
            if (_zone == null)
                return;

            var worldPosition = _zone.Center.FromXY();
            var screenPosition = UnityEngine.Camera.main!.WorldToScreenPoint(worldPosition);
            productionZonePopup.gameObject.transform.position = screenPosition;
        }

        public void Unbind()
        {
            productionZonePopup.Reset();
            productionZonePopup.Hide();
            _zone = null;
        }
    }
}