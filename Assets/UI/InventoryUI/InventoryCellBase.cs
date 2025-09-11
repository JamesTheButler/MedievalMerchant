using System;
using Data;
using Data.Configuration;
using Data.Setup;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public abstract class InventoryCellBase : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler,
        IPointerExitHandler
    {
        public event Action Clicked;
        public event Action RightClicked;

        public Good? Good { get; private set; }

        [SerializeField, Required]
        private Image goodIcon;

        [SerializeField, Required]
        private Image disabledIcon;

        [SerializeField]
        private TMP_Text amountText;

        [SerializeField, Required]
        private TooltipHandler tooltipHandler;

        private readonly Lazy<GoodsConfig> _goodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        public void SetGood(Good? good)
        {
            Good = good;
            if (good == null)
            {
                goodIcon.gameObject.SetActive(false);
                return;
            }

            var goodConfigData = _goodsConfig.Value.ConfigData[good!.Value];
            goodIcon.gameObject.SetActive(true);
            goodIcon.sprite = goodConfigData.Icon;
            tooltipHandler.SetTooltip(goodConfigData.GoodName);
        }

        public void SetAmount(int amount)
        {
            if (amount <= 0)
            {
                amountText?.gameObject.SetActive(false);
                disabledIcon.gameObject.SetActive(true);
            }
            else
            {
                amountText?.gameObject.SetActive(true);
                disabledIcon.gameObject.SetActive(false);
                amountText?.SetText(amount.ToString());
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            switch (eventData.button)
            {
                case PointerEventData.InputButton.Left: Clicked?.Invoke(); break;
                case PointerEventData.InputButton.Right: RightClicked?.Invoke(); break;
            }
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            tooltipHandler.SetEnabled(Good != null);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            tooltipHandler.SetEnabled(false);
        }
    }
}