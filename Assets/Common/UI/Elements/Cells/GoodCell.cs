using System;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Goods.Config;
using Features.Goods.UI;
using JetBrains.Annotations;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI.Elements.Cells
{
    public class GoodCell : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;
        public event Action RightClicked;

        public Good? Good { get; private set; }

        [SerializeField, Required]
        private Image goodIcon, cornerIcon, background;

        [SerializeField]
        protected GoodTooltipHandler tooltipHandler;

        [SerializeField, Required]
        protected SimpleTooltipHandler messageTooltip;

        [SerializeField]
        private float disabledAlpha = 0.5f;

        protected readonly Lazy<GoodResources> GoodsConfig = new(() => ResourceManager.Instance.GoodResources);

        private void Awake()
        {
            if (cornerIcon.sprite == null)
            {
                cornerIcon.gameObject.SetActive(false);
            }
        }

        public void SetGood(Good? good)
        {
            if (Good == good)
                return;

            Good = good;

            tooltipHandler?.SetEnabled(good != null);

            if (good == null)
            {
                goodIcon.gameObject.SetActive(false);
                OnSetGood(null);
                return;
            }

            var goodConfigData = GoodsConfig.Value.ResourceData[good!.Value];
            goodIcon.gameObject.SetActive(true);
            goodIcon.sprite = goodConfigData.Icon;
            tooltipHandler?.SetData(good.Value);

            OnSetGood(good);
        }

        public void ChangeBackground(Sprite image, Color? color = null)
        {
            background.sprite = image;
            background.color = color ?? Color.white;
        }

        protected virtual void OnSetGood(Good? good) { }

        public void SetEnabled(bool isEnabled)
        {
            goodIcon.color = goodIcon.color.WithAlpha(isEnabled ? 1f : disabledAlpha);
        }

        public void EnableCornerIcon(bool isEnabled = true, Sprite icon = null)
        {
            cornerIcon.gameObject.SetActive(isEnabled);

            if (icon != null)
            {
                cornerIcon.sprite = icon;
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

        public void PostMessage(string message)
        {
            messageTooltip.ShowTooltip(message);
        }
    }
}