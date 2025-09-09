using Common;
using Data.Configuration;
using Data.Setup;
using Map;
using UnityEngine;

namespace UI
{
    public sealed class ZonePopupHandler : MonoBehaviour
    {
        [SerializeField]
        private ZonePopup zonePopup;

        [SerializeField]
        private Grid grid;

        private RecipeConfig _recipeConfig;

        private void Start()
        {
            _recipeConfig = ConfigurationManager.Instance.RecipeConfig;
            Unbind();
        }

        public void Bind(ProductionZone zone)
        {
            if (zone == null)
            {
                Unbind();
                return;
            }

            zonePopup.Reset();
            var worldPosition = zone.Center.FromXY();
            // BUG: this is not update when we move the camera
            var screenPosition = Camera.main!.WorldToScreenPoint(worldPosition);
            zonePopup.gameObject.transform.position = screenPosition;

            foreach (var tier1Good in zone.AvailableGoods)
            {
                var tier2Good = _recipeConfig.Tier2Recipes[tier1Good];
                zonePopup.AddGood(tier1Good, tier2Good);
            }

            zonePopup.Show();
        }

        public void Unbind()
        {
            zonePopup.Reset();
            zonePopup.Hide();
        }
    }
}