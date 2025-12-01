using System;
using Common;
using Features.Map.Tiling;
using Infrastructure;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField, Required]
    private new Camera camera;

    [SerializeField]
    private UnityEvent cameraMoved, cameraZoomed;

    [SerializeField, Required]
    private TilemapManager tilemapManager;

    [SerializeField]
    private float startupPadding;

    [SerializeField]
    private float zoomSpeed = 1;

    [SerializeField]
    private float keyboardPanSpeedPixelPerSecond = 1;

    [SerializeField]
    private float zLevel = -10;

    [SerializeField]
    private float minSize = 1;

    [SerializeField]
    private Vector2 safeArea;

    private float _maxSize = 10;
    private Vector2 _lastMousePosition;
    private Vector2 _lastKeyInputs = Vector2.zero;
    private bool _isPanning;
    private Bounds _bounds;

    private void Start()
    {
        // force orthographic camera
        if (camera.orthographic)
            return;

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
        // TODO - POLISH: camera size should fit both dimensions. this depends on the aspect ratio & account for ui 
        var mapSize = GameplayContext.Model.TileFlagMap.Size.y;
        camera.orthographicSize = mapSize * .5f + startupPadding;
        _maxSize = camera.orthographicSize * 1.5f;
        _bounds = tilemapManager.Tilemap.localBounds;
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        var scrollValue = -context.ReadValue<Vector2>().y;
        var newSize = camera.orthographicSize + scrollValue * zoomSpeed;
        camera.orthographicSize = Math.Clamp(newSize, minSize, _maxSize);
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
        _isPanning = context.ReadValueAsButton();
    }

    private void ApplyMapMovementKeys()
    {
        Pan(_lastKeyInputs * keyboardPanSpeedPixelPerSecond);
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