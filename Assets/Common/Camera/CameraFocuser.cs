using System;
using Common.Infrastructure;
using Features.Towns;
using UnityEngine;

namespace Common.Camera
{
    public sealed class CameraFocuser : MonoBehaviour
    {
        private CameraManager _cameraManager;

        private void Start()
        {
            _cameraManager = FindAnyObjectByType<CameraManager>();
            GameplayContext.Instance.Services.CameraService.FocusCameraRequested += FocusCameraOnTown;
        }

        public void FocusCameraOnPlayer()
        {
            var playerLocation = GameplayContext.Instance.Model.Player.Location;
            _cameraManager.FocusCamera(playerLocation.WorldLocation.Value);
        }

        private void FocusCameraOnTown(Town town)
        {
            _cameraManager.FocusCamera(town.WorldLocation);
        }
    }
}