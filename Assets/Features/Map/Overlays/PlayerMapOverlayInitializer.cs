using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class PlayerMapOverlaySetup : InitializableBehavior
    {
        [SerializeField, Required]
        private MapEntityOverlay playerOverlay;

        public override void Initialize()
        {
            playerOverlay.SetUp(
                GameplayContext.Instance.Model.Player.Location,
                gameObject.transform.position.z);
        }

        public override void CleanUp()
        {
            playerOverlay.CleanUp();
            base.CleanUp();
        }
    }
}