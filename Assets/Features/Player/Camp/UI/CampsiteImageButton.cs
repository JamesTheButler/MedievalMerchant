using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteImageButton :
        MonoBehaviour,
        IPointerClickHandler
    {
        [SerializeField, Required]
        private Image mainImage;

        [SerializeField]
        private UnityEvent clicked;

        private void Awake()
        {
            mainImage.alphaHitTestMinimumThreshold = 0.1f;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked.Invoke();
        }
    }
}