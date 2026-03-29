using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements;
using Features.Map.Pathfinding;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteUIHandler : InitializableBehavior
    {
        [SerializeField, Required]
        private CampsitePanelUI campsitePanelUI;

        private readonly Bindings _bindings = new();

        public override void Initialize()
        {
            var context = GameplayContext.Instance;
            if (context.Model.Camp == null) return;

            _bindings.Track(
                context.Selection.CampSelected.Observe(OnCampSelectionChanged),
                context.Model.Player.Location.MapLocation.Observe(OnPlayerLocationChanged)
            );
        }

        public override void CleanUp()
        {
            _bindings.UnbindAll();
        }

        private void OnCampSelectionChanged(bool selected)
        {
            if (selected)
            {
                campsitePanelUI.Open();
            }
            else
            {
                campsitePanelUI.Close();
            }
        }

        private void OnPlayerLocationChanged(IMapLocation location)
        {
            var isAtCamp = location is Logic.Camp;
            campsitePanelUI.SetInteractable(isAtCamp);
            SetBlockerActive(!isAtCamp);
        }

        private void SetBlockerActive(bool active)
        {
            // TBD
        }
    }
}