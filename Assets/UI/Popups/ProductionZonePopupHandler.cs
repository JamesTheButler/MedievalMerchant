using Common;
using Data.Configuration;
using Data.Setup;
using Map;
using UnityEngine;

namespace UI
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
            // BUG: this is not update when we move the camera
            var screenPosition = Camera.main!.WorldToScreenPoint(worldPosition);
            productionZonePopup.gameObject.transform.position = screenPosition;

            foreach (var tier1Good in zone.AvailableGoods)
            {
                var tier2Good = _recipeConfig.Tier2Recipes[tier1Good];
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