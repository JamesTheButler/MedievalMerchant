using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common.UI.Elements
{
    public sealed class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        public event Action Hovered, Unhovered;

        [SerializeField]
        private bool unhoverOnAwake;

        [SerializeField]
        private UnityEvent onHoverStart, onHoverEnd;

        private void Awake()
        {
            if (!unhoverOnAwake) return;

            onHoverEnd.Invoke();
            Unhovered?.Invoke();
        }

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