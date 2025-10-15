using System;
using Common;
using Common.Types;
using Features.Goods.Config;
using Features.Goods.UI;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public class GoodCell : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;
        public event Action RightClicked;

        public Good? Good { get; private set; }

        [SerializeField, Required]
        private Image goodIcon;

        [SerializeField, Required]
        private Image disabledIcon;

        [SerializeField, Required]
        private Image cornerIcon;

        [SerializeField, Required]
        protected GoodTooltipHandler tooltipHandler;

        protected readonly Lazy<GoodsConfig> GoodsConfig = new(() => ConfigurationManager.Instance.GoodsConfig);

        private void Awake()
        {
            if (cornerIcon.sprite == null)
            {
                cornerIcon.gameObject.SetActive(false);
            }
        }

        public void SetGood(Good? good)
        {
            Good = good;

            tooltipHandler.SetEnabled(good != null);

            if (good == null)
            {
                goodIcon.gameObject.SetActive(false);
                OnSetGood(null);
                return;
            }

            var goodConfigData = GoodsConfig.Value.ConfigData[good!.Value];
            goodIcon.gameObject.SetActive(true);
            goodIcon.sprite = goodConfigData.Icon;
            tooltipHandler.SetTooltip(good.Value);

            OnSetGood(good);
        }

        protected virtual void OnSetGood(Good? good) { }

        public void SetEnabled(bool isEnabled)
        {
            disabledIcon.gameObject.SetActive(!isEnabled);
        }

        public void SetCornerIcon(Sprite icon)
        {
            cornerIcon.gameObject.SetActive(true);
            cornerIcon.sprite = icon;
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