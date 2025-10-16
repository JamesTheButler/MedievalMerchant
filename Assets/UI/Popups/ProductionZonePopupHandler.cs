using Common;
using Common.Types;
using Features.Goods.Config;
using Features.Map;
using UnityEngine;

namespace UI.Popups
{
    public sealed class ProductionZonePopupHandler : MonoBehaviour
    {
        [SerializeField]
        private ProductionZonePopup productionZonePopup;

        [SerializeField]
        private Grid grid;

        private RecipeConfig _recipeConfig;
        private ProductionZone _zone;

        private void Start()
        {
            _recipeConfig = ConfigurationManager.Instance.RecipeConfig;
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
            var worldPosition = zone.Center.FromXY();

            // BUG: this is not updated when we move the camera
            var screenPosition = Camera.main!.WorldToScreenPoint(worldPosition);
            productionZonePopup.gameObject.transform.position = screenPosition;

            // TODO - HACK: a prod zone should have exactly one associated region.
            productionZonePopup.SetRegion(zone.Regions.GetRandom());
            foreach (var tier1Good in zone.AvailableGoods)
            {
                var tier2Good = _recipeConfig.GetTier2RecipeForComponent(tier1Good).Result;
                productionZonePopup.AddGood(tier1Good, tier2Good);
            }

            productionZonePopup.Show();
        }

        public void Unbind()
        {
            productionZonePopup.Reset();
            productionZonePopup.Hide();
            _zone = null;
        }
    }
}