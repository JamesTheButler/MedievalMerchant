using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

namespace Common.UI
{
    public sealed class Clickable : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private UnityEvent clicked;

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked.Invoke();
        }
    }
}