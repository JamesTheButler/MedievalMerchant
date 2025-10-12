using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common.UI
{
    public sealed class Hoverable : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private UnityEvent onHoverStart, onHoverEnd;

        public void OnPointerEnter(PointerEventData eventData)
        {
            onHoverStart.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            onHoverEnd.Invoke();
        }
    }
}