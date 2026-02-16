using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Features.Player.Camp.UI
{
    public sealed class CampsiteImageButton :
        MonoBehaviour,
        IPointerEnterHandler,
        IPointerExitHandler,
        IPointerClickHandler
    {
        [SerializeField, Required]
        private GameObject outline;
        [SerializeField, Required]
        private Image mainImage;

        [SerializeField]
        private UnityEvent clicked;

        private void Awake()
        {
            outline.SetActive(false);
            mainImage.alphaHitTestMinimumThreshold = 0.1f;
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            // outline.gameObject.SetActive(true);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            // outline.gameObject.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            clicked.Invoke();
        }
    }
}