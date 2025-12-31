using System;
using Common.Infrastructure;
using Features.Towns;
using UnityEngine;

namespace Common.Camera
{
    public sealed class CameraFocuser : MonoBehaviour
    {
        private readonly Lazy<CameraManager> _cameraManager = new(FindAnyObjectByType<CameraManager>);

        private void Start()
        {
            GameplayContext.Instance.Services.CameraService.FocusCameraRequested += FocusCameraOnTown;
        }

        public void FocusCameraOnPlayer()
        {
            var playerLocation = GameplayContext.Instance.Model.Player.Location;
            _cameraManager.Value.FocusCamera(playerLocation.WorldLocation.Value);
        }

        private void FocusCameraOnTown(Town town)
        {
            _cameraManager.Value.FocusCamera(town.WorldLocation);
        }
    }
}