using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using Common.Utility;
using Features.Map.Pathfinding;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class PlayerOverla
    
    public sealed class PlayerOverlay : InitializableBehavior
    {
        [SerializeField, Required]
        private GameObject worldOverlay, townOverlay;

        [SerializeField, Required]
        private new Animation animation;

        private readonly GameSpeedAnimationHandler _animationHandler = new();

        private IMapEntity _mapEntity;
        private float _zLevel;

        public override void Initialize()
        {
            SetUp(
                GameplayContext.Instance.Model.Player.Location,
                gameObject.transform.position.z);
        }

        public void SetUp(IMapEntity mapEntity, float zLevel)
        {
            _mapEntity = mapEntity;
            _zLevel = zLevel;


            _animationHandler.Initialize(animation);

            _mapEntity.MapLocation.Observe(OnLocationChanged);
            _mapEntity.WorldLocation.Observe(OnWorldLocationChanged);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _mapEntity.MapLocation.StopObserving(OnLocationChanged);
            _mapEntity.WorldLocation.StopObserving(OnWorldLocationChanged);
            _animationHandler.CleanUp();
        }

        private void OnWorldLocationChanged(Vector2 worldLocation)
        {
            if (_mapEntity.MapLocation.Value != null)
                return;

            gameObject.transform.localPosition = worldLocation.FromXY(_zLevel);
        }

        private void OnLocationChanged(IMapLocation location)
        {
            var isAtLocation = location != null;

            townOverlay.SetActive(isAtLocation);
            worldOverlay.SetActive(!isAtLocation);

            if (!isAtLocation)
                return;

            gameObject.transform.localPosition = location.WorldLocation.FromXY(_zLevel);
        }
    }
}