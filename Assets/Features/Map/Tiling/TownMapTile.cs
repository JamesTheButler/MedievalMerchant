using System;
using Common.UI.Art;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.Map.Tiling
{
    public sealed class TownMapTile : MapTile, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
    {
        public event Action LeftClicked, RightClicked, Hovered, Unhovered;

        [SerializeField, Required]
        private GameObject selectionOutline;

        [field: SerializeField, Required]
        public Transform OverlayAnchor { get; private set; }

        [SerializeField, Required]
        private AudioSource upgradeAudioSource;

        [SerializeField, Required]
        private SimpleAnimatorHandler animatorHandler;

        private void Awake()
        {
            selectionOutline.SetActive(false);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left:
                    LeftClicked?.Invoke();
                    break;
                case PointerEventData.InputButton.Right:
                    RightClicked?.Invoke(); break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            selectionOutline.SetActive(true);
            Hovered?.Invoke();
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            selectionOutline.SetActive(false);
            Unhovered?.Invoke();
        }

        public void PlayUpgradeEffects()
        {
            upgradeAudioSource.Play();
            animatorHandler.Play();
        }
    }
}