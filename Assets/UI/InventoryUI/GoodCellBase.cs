using System;
using Data;
using Data.Configuration;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public class GoodCellBase : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;
        public event Action RightClicked;

        public Good? Good { get; private set; }

        [SerializeField, Required]
        private Image goodIcon;

        [SerializeField, Required]
        private Image disabledIcon;

        [SerializeField, Required]
        protected TooltipHandler tooltipHandler;

        protected readonly Lazy<GoodsConfig> GoodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        public void SetGood(Good? good)
        {
            Good = good;

            tooltipHandler.SetEnabled(good != null);

            if (good == null)
            {
                goodIcon.gameObject.SetActive(false);
                return;
            }

            var goodConfigData = GoodsConfig.Value.ConfigData[good!.Value];
            goodIcon.gameObject.SetActive(true);
            goodIcon.sprite = goodConfigData.Icon;
            tooltipHandler.SetTooltip(goodConfigData.GoodName);
        }

        public void SetEnabled(bool isEnabled)
        {
            disabledIcon.gameObject.SetActive(!isEnabled);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left: Clicked?.Invoke(); break;
                case PointerEventData.InputButton.Right: RightClicked?.Invoke(); break;
            }
        }
    }
}