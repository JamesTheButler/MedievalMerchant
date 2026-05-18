using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI.Elements
{
    public sealed class HoverForwarder : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private GameObject target;

        public void OnPointerEnter(PointerEventData eventData)
        {
            ExecuteEvents.Execute(
                target,
                eventData,
                ExecuteEvents.pointerEnterHandler
            );
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            ExecuteEvents.Execute(
                target,
                eventData,
                ExecuteEvents.pointerExitHandler
            );
        }
    }
}