using Common.Utility;
using Features.Map.Pathfinding;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class MapEntityOverlay : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject travellingIcon, landedIcon;

        [SerializeField, Required]
        private new Animation animation;

        [SerializeField, Required]
        private Transform travellingIconAnchor, landedIconAnchor;

        [SerializeField, Required]
        private OverlayIconGroup iconGroup;

        private readonly GameSpeedAnimationHandler _animationHandler = new();

        private IMapEntity _mapEntity;
        private float _zLevel;

        public void SetUp(IMapEntity mapEntity, float zLevel)
        {
            _mapEntity = mapEntity;
            _zLevel = zLevel;

            _animationHandler.Initialize(animation);

            _mapEntity.MapLocation.Observe(OnLocationChanged);
            _mapEntity.WorldLocation.Observe(OnWorldLocationChanged);
        }

        public void CleanUp()
        {
            _mapEntity.MapLocation.StopObserving(OnLocationChanged);
            _mapEntity.WorldLocation.StopObserving(OnWorldLocationChanged);
            _animationHandler.CleanUp();
        }

        public void AddIcon(Sprite icon)
        {
            iconGroup.AddIcon(icon);
        }

        public void RemoveIcon(Sprite icon)
        {
            iconGroup.RemoveIcon(icon);
        }

        private void OnWorldLocationChanged(Vector2 worldLocation)
        {
            if (_mapEntity.MapLocation.Value != null)
                return;

            gameObject.transform.localPosition = worldLocation.FromXY(_zLevel);
        }

        private void OnLocationChanged(IMapLocation location)
        {
            var isLanded = location != null;

            landedIcon.SetActive(isLanded);
            travellingIcon.SetActive(!isLanded);

            var iconGroupAnchor = isLanded ? landedIconAnchor : travellingIconAnchor;
            iconGroup.transform.localPosition = iconGroupAnchor.localPosition;

            if (!isLanded)
                return;

            gameObject.transform.localPosition = location.WorldLocation.FromXY(_zLevel);
        }
    }
}