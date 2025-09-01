using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraManager : MonoBehaviour
{
    [SerializeField]
    private new Camera camera;

    [SerializeField]
    private float startupPadding;

    [SerializeField]
    private float zoomSpeed;

    [SerializeField]
    private float minSize = 1;

    private float _maxSize = 10;

    private void Start()
    {
        // force orthographic camera
        if (camera.orthographic) return;
        camera.orthographic = true;
    }

    public void FitMapSize(float mapSize)
    {
        camera.orthographicSize = mapSize * .5f + startupPadding;
        _maxSize = camera.orthographicSize * 1.5f;
    }

    public void OnScrollWheel(InputAction.CallbackContext context)
    {
        var scrollValue = -context.ReadValue<Vector2>().y;
        var newSize = camera.orthographicSize + scrollValue * zoomSpeed;
        camera.orthographicSize = Math.Clamp(newSize, minSize, _maxSize);
    }
}