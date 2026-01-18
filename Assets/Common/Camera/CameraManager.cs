using System;
using Common.Infrastructure.Gameplay;
using Common.UI.Utility;
using Common.Utility;
using Features.Map.Tiling;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace Common.Camera
{
    public sealed class CameraManager : MonoBehaviour
    {
        [SerializeField, Required]
        private new UnityEngine.Camera camera;

        [SerializeField]
        private UnityEvent cameraMoved, cameraZoomed;

        [SerializeField, Required]
        private TilemapManager tilemapManager;

        [SerializeField]
        private float startSize;

        [SerializeField]
        private float zoomSpeed = 1;

        [SerializeField]
        private float keyboardPanSpeedMinZoom = 10, keyboardPanSpeedMaxZoom = 1;

        [SerializeField]
        private float zLevel = -10;

        [SerializeField]
        private float minSize = 1;

        [SerializeField]
        private Vector2 safeArea;

        [SerializeField]
        private float cameraFitPaddingFactor = 1.5f;

        private float _maxSize = 10;
        private float _keyboardPanSpeed = 1;
        private Vector2 _lastMousePosition;
        private Vector2 _lastKeyInputs = Vector2.zero;
        private bool _isPanning;
        private Bounds _bounds;

        private void Start()
        {
            RefreshKeyboardPanSpeed();

            camera.orthographic = true;
        }

        public void FixedUpdate()
        {
            if (_lastKeyInputs == Vector2.zero)
                return;

            ApplyMapMovementKeys();
        }

        public void FitMapSize()
        {
            var mapHalfSize = GameplayContext.Instance.Model.TileFlagMap.Size.y * .5f;
            camera.orthographicSize = mapHalfSize;
            _maxSize = mapHalfSize * 1.5f;
            _bounds = tilemapManager.Tilemap.localBounds;
        }

        public void OnScrollWheel(InputAction.CallbackContext context)
        {
            var scrollValue = -context.ReadValue<Vector2>().y;
            var newSize = camera.orthographicSize + scrollValue * zoomSpeed;
            camera.orthographicSize = Math.Clamp(newSize, minSize, _maxSize);
            RefreshKeyboardPanSpeed();
            cameraZoomed?.Invoke();
        }

        public void OnMouseMoved(InputAction.CallbackContext context)
        {
            var newMousePosition = context.ReadValue<Vector2>();
            var oldMousePosition = _lastMousePosition;
            _lastMousePosition = newMousePosition;

            if (!_isPanning)
                return;

            var delta = oldMousePosition - newMousePosition;
            Pan(delta);
        }

        public void OnMapMovementKeys(InputAction.CallbackContext context)
        {
            _lastKeyInputs = context.ReadValue<Vector2>(); // -1..1
        }

        public void InitiateOrAbortPan(InputAction.CallbackContext context)
        {
            if (UIUtility.IsPointerOverBlockingUI())
                return;

            _isPanning = context.ReadValueAsButton();
        }

        public void FocusCamera(Vector2 worldPosition)
        {
            camera.transform.position = worldPosition.FromXY(camera.transform.position.z);
        }

        private void RefreshKeyboardPanSpeed()
        {
            var cameraSizeT = Mathf.InverseLerp(minSize, _maxSize, camera.orthographicSize);
            _keyboardPanSpeed = Mathf.Lerp(keyboardPanSpeedMaxZoom, keyboardPanSpeedMinZoom, cameraSizeT);
        }

        private void ApplyMapMovementKeys()
        {
            Pan(_lastKeyInputs * _keyboardPanSpeed);
        }

        private void Pan(Vector2 delta)
        {
            var worldUnitsPerPixel = camera.orthographicSize * 2f / Screen.height;

            var worldDelta = new Vector3(
                delta.x * worldUnitsPerPixel,
                delta.y * worldUnitsPerPixel,
                0f);

            var targetPosition = camera.transform.position + worldDelta;

            camera.transform.position = targetPosition
                .Clamp(_bounds)
                .WithOverrides(z: zLevel);

            cameraMoved?.Invoke();
        }

        private void OnDrawGizmosSelected()
        {
            Gizmos.DrawWireCube(camera.transform.position, safeArea.FromXY(1));
        }
    }
}