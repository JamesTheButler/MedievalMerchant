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
        private Vector2 _origin;

        private void Start()
        {
            _playerLocation = Model.Instance.Player.Location;
            _origin = Model.Instance.TileFlagMap.Origin;

            _playerLocation.TownEntered += OnTownEntered;
            _playerLocation.TownExited += OnTownExited;
            _playerLocation.WorldLocationChanged += OnWorldLocationChanged;

            OnTownEntered(_playerLocation.CurrentTown);
            OnWorldLocationChanged(_playerLocation.WorldLocation);
        }

        private void OnWorldLocationChanged(Vector2 worldLocation)
        {
            if (_playerLocation.CurrentTown != null) return;

            gameObject.transform.position = worldLocation + _origin;
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
            gameObject.transform.localPosition = town.WorldLocation + _origin;
        }

        private void OnTownExited(Town town)
        {
            townOverlay.SetActive(false);
            worldOverlay.SetActive(true);
        }
    }
}