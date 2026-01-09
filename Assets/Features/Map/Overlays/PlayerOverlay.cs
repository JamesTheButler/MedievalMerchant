using Common.Infrastructure;
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

            _playerLocation.TownEntered += OnTownEntered;
            _playerLocation.TownExited += OnTownExited;
            _animationHandler.Initialize(animation);

            _playerLocation.WorldLocation.Observe(OnWorldLocationChanged);

            OnTownEntered(_playerLocation.CurrentTown);
        }

        private void OnDestroy()
        {
            _playerLocation.TownEntered -= OnTownEntered;
            _playerLocation.TownExited -= OnTownExited;
            _playerLocation.WorldLocation.StopObserving(OnWorldLocationChanged);
            _animationHandler.CleanUp();
        }

        private void OnWorldLocationChanged(Vector2 worldLocation)
        {
            if (_playerLocation.CurrentTown != null) return;

            gameObject.transform.localPosition = worldLocation.FromXY(_zLevel);
        }

        private void OnTownEntered(Town town)
        {
            if (town == null)
            {
                OnTownExited(null);
                return;
            }

            townOverlay.SetActive(true);
            worldOverlay.SetActive(false);
            gameObject.transform.localPosition = town.WorldLocation.FromXY(_zLevel);
        }

        private void OnTownExited(Town town)
        {
            townOverlay.SetActive(false);
            worldOverlay.SetActive(true);
        }
    }
}