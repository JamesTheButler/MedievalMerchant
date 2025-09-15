using System;
using Common;
using Data;
using Data.Towns;
using NaughtyAttributes;
using UnityEngine;

namespace Map.Overlays
{
    public sealed class PlayerOverlay : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject worldOverlay;

        [SerializeField, Required]
        private GameObject townOverlay;

        private PlayerLocation _playerLocation;
        private float _zLevel;

        private void Start()
        {
            _playerLocation = Model.Instance.Player.Location;
            _zLevel = gameObject.transform.position.z;

            _playerLocation.TownEntered += OnTownEntered;
            _playerLocation.TownExited += OnTownExited;
            _playerLocation.WorldLocation.Observe(OnWorldLocationChanged);

            OnTownEntered(_playerLocation.CurrentTown);
        }

        private void OnDestroy()
        {
            _playerLocation.TownEntered -= OnTownEntered;
            _playerLocation.TownExited -= OnTownExited;
            _playerLocation.WorldLocation.StopObserving(OnWorldLocationChanged);
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