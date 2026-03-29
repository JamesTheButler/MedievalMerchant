using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using Common.Utility;
using Features.Map.Pathfinding;
using Features.Player.Logic;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class PlayerOverlay : InitializableBehavior
    {
        [SerializeField, Required]
        private GameObject worldOverlay, townOverlay;

        [SerializeField, Required]
        private new Animation animation;

        private readonly GameSpeedAnimationHandler _animationHandler = new();

        private PlayerLocation _playerLocation;
        private float _zLevel;

        public override void Initialize()
        {
            _playerLocation = GameplayContext.Instance.Model.Player.Location;
            _zLevel = gameObject.transform.position.z;

            _animationHandler.Initialize(animation);

            _playerLocation.MapLocation.Observe(OnLocationChanged);
            _playerLocation.WorldLocation.Observe(OnWorldLocationChanged);
        }

        public override void CleanUp()
        {
            base.CleanUp();
            _playerLocation.MapLocation.StopObserving(OnLocationChanged);
            _playerLocation.WorldLocation.StopObserving(OnWorldLocationChanged);
            _animationHandler.CleanUp();
        }

        private void OnWorldLocationChanged(Vector2 worldLocation)
        {
            if (_playerLocation.MapLocation.Value != null)
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