using UnityEngine;

namespace UI
{
    [ExecuteAlways]
    public sealed class BackgroundQuadScaler : MonoBehaviour
    {
        [SerializeField]
        private Camera cam;


        private void Start()
        {
            if (cam == null)
            {
                Debug.LogError("Camera not assigned.");
            }
        }

        private void Update()
        {
            if (cam == null)
                return;

            var height = cam.orthographicSize * 2f;
            var width = height * cam.aspect;

            transform.localScale = new Vector3(width, height, 1f);
        }
    }
}