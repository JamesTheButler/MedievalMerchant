using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI.Elements
{
    public sealed class ClickForwarder : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private GameObject target;

        public void OnPointerClick(PointerEventData eventData)
        {
            ExecuteEvents.Execute(
                target,
                eventData,
                ExecuteEvents.pointerClickHandler
            );
        }
    }
}
