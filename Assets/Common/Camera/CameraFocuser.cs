using Common.Infrastructure;
using UnityEngine;

namespace Common.Camera
{
    public sealed class CameraFocuser : MonoBehaviour
    {
        public void FocusCameraOnPlayer()
        {
            var playerLocation = GameplayContext.Instance.Model.Player.Location;
            var camMgr = FindAnyObjectByType<CameraManager>();
            camMgr.FocusCamera(playerLocation.WorldLocation.Value);
        }
    }
}