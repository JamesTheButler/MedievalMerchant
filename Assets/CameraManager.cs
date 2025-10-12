using System;
using Common;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField, Required]
    private new Camera camera;

    [SerializeField]
    private float startupPadding;

    [SerializeField]
    private float zoomSpeed = 1;

    [SerializeField]
    private float mousePanSpeed = 1;

    [SerializeField]
    private float keyboardPanSpeed = 1;

    [SerializeField]
    private float minSize = 1;

    private float _maxSize = 10;
    private Vector2 _lastMousePosition;
    private bool _isPanning;

    private Bounds _bounds;

    private void Start()
    {
        // force orthographic camera
        if (camera.orthographic) return;
        camera.orthographic = true;
    }

    public void FitMapSize()
    {
        // TODO - POLISH: camera size should fit both dimensions. this depends on the aspect ratio & account for ui 
        var mapSize = Model.Instance.TileFlagMap.Size.y;
        camera.orthographicSize = mapSize * .5f + startupPadding;
        _maxSize = camera.orthographicSize * 1.5f;

        _bounds = new Bounds(Vector3.zero, Vector3.one * _maxSize);
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        var scrollValue = -context.ReadValue<Vector2>().y;
        var newSize = camera.orthographicSize + scrollValue * zoomSpeed;
        camera.orthographicSize = Math.Clamp(newSize, minSize, _maxSize);
    }

    // TODO - POLISH: mousePanSpeed should scale with size so that the mouse seems to be perfectly attached to the
    //    map and not have any glide
    // TODO - POLISH: vertical and horizontal panning do not have the same speed for some reason
    public void OnMouseMoved(InputAction.CallbackContext context)
    {
        var newMousePosition = context.ReadValue<Vector2>();
        var oldMousePosition = _lastMousePosition;
        _lastMousePosition = newMousePosition;

        if (!_isPanning) return;

        var delta = oldMousePosition - newMousePosition;
        Pan(delta * mousePanSpeed);
    }

    // TODO - POLISH: this doesn't work. method is only invoked OnKeyUp and OnKeyDown, not OnKeyHeld
    public void OnMapMovementKeys(InputAction.CallbackContext context)
    {
        var delta = context.ReadValue<Vector2>();
        Pan(delta * keyboardPanSpeed);
    }

    public void InitiateOrAbortPan(InputAction.CallbackContext context)
    {
        _isPanning = context.ReadValueAsButton();
    }

    private void Pan(Vector2 delta)
    {
        var deltaVector = new Vector3(
            delta.x / Screen.currentResolution.width,
            delta.y / Screen.currentResolution.height,
            0f);

        camera.transform.position = VectorExtensions.Clamp(camera.transform.position + deltaVector, _bounds);
    }
}