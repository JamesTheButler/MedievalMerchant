using UnityEngine;
using UnityEngine.EventSystems;

namespace Common.UI.Elements
{
    public sealed class Linkable : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField]
        private string link;

        public void OnPointerClick(PointerEventData eventData)
        {
            Application.OpenURL(link);
        }
    }
}