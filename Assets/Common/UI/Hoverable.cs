using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common.UI
{
    public sealed class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private UnityEvent onHoverStart, onHoverEnd;

        public event Action Hovered, Unhovered;

        public void OnPointerEnter(PointerEventData eventData)
        {
            onHoverStart.Invoke();
            Hovered?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onHoverEnd.Invoke();
            Unhovered?.Invoke();
        }
    }
}