using System;
using Common.Infrastructure;
using Common.Types;
using Features.Towns.Flags.Logic;
using Features.Towns.Flags.UI;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Features.StartMenu.UI
{
    public sealed class AllySelectionToggle : MonoBehaviour, IPointerClickHandler
    {
        public event Action<Region> Selected;

        [field: SerializeField]
        public Region Region { get; private set; }

        [SerializeField, Required]
        private FlagRenderer flagRenderer;

        [SerializeField, Required]
        private TMP_Text regionTitle;

        [SerializeField, Required]
        private GameObject selectionFrame;

        private void Awake()
        {
            var flagFactory = new FlagFactory();
            flagRenderer.SetFlag(flagFactory.CreateFlagInfo(Region));

            var regionResources = ResourceManager.Instance.RegionResources;
            var regionData = regionResources.Data[Region];
            regionTitle.text = regionData.Name;
        }

        public void Toggle(bool isToggled)
        {
            selectionFrame.SetActive(isToggled);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button != PointerEventData.InputButton.Left)
                return;

            Selected?.Invoke(Region);
        }
    }
}