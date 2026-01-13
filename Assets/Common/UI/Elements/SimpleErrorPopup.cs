using Common.Utility;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Common.UI.Elements
{
    public sealed class SimpleErrorPopup : MonoBehaviour
    {
        public void SetUp(string message)
        {
            var tmpText = FindFirstObjectByType<TMP_Text>();
            tmpText.text = message;
        }

        private void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.WasAnyKeyPressedThisFrame())
            {
                Destroy(gameObject);
            }
        }
    }
}