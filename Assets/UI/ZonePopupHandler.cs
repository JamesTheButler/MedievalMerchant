using Common;
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

        private void Start()
        {
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

            foreach (var good in zone.AvailableGoods)
            {
                zonePopup.AddGood(good, good);
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