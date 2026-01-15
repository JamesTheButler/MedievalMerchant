using Common.Infrastructure.Gameplay;
using Common.Utility;
using Features.Player.Logic;
using Features.Towns;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Map.Overlays
{
    public sealed class PlayerOverlay : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject worldOverlay, townOverlay;

        [SerializeField, Required]
        private new Animation animation;

        private PlayerLocation _playerLocation;
        private readonly GameSpeedAnimationHandler _animationHandler = new();
        private float _zLevel;

        private void Start()
        {
            _playerLocation = GameplayContext.Instance.Model.Player.Location;
            _zLevel = gameObject.transform.position.z;

            _animationHandler.Initialize(animation);

            _playerLocation.CurrentTown.Observe(OnTownChanged);
            _playerLocation.WorldLocation.Observe(OnWorldLocationChanged);
        }

        private void OnDestroy()
        {
            _playerLocation.CurrentTown.StopObserving(OnTownChanged);
            _playerLocation.WorldLocation.StopObserving(OnWorldLocationChanged);
            _animationHandler.CleanUp();
        }

        private void OnWorldLocationChanged(Vector2 worldLocation)
        {
            if (_playerLocation.CurrentTown.Value != null)
                return;

            gameObject.transform.localPosition = worldLocation.FromXY(_zLevel);
        }

        private void OnTownChanged(Town town)
        {
            var isInTown = town != null;

            townOverlay.SetActive(isInTown);
            worldOverlay.SetActive(!isInTown);

            if (!isInTown)
                return;

            gameObject.transform.localPosition = town.WorldLocation.FromXY(_zLevel);
        }
    }
}