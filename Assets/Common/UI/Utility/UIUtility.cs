using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

namespace Common.UI.Utility
{
    public static class UIUtility
    {
        public static bool IsPointerOverBlockingUI()
        {
            var raycastResults = new List<RaycastResult>();
            var blockingUiMask = LayerMask.GetMask("UI");
            var eventSystem = EventSystem.current;
            if (eventSystem == null) return false;

            var pointerPosition = Mouse.current.position.ReadValue();
            var pointerEventData = new PointerEventData(eventSystem) { position = pointerPosition };

            eventSystem.RaycastAll(pointerEventData, raycastResults);

            foreach (var result in raycastResults)
            {
                var hitObject = result.gameObject;
                var hitLayerMaskBit = 1 << hitObject.layer;

                if ((blockingUiMask & hitLayerMaskBit) == 0)
                    continue;

                return true;
            }

            return false;
        }
    }
}