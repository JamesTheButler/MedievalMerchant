using System;
using Data;
using Data.Configuration;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public sealed class InventoryCell : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        public Good? Good { get; private set; }

        [SerializeField, Required]
        private Image goodIcon;

        [SerializeField, Required]
        private Image disabledIcon;

        [SerializeField, Required]
        private Image productionIcon;

        [SerializeField, Required]
        private TMP_Text amountText;

        private bool _showAmountText = true;
        
        public void SetGood(Good? good)
        {
            Good = good;
            if (good == null)
            {
                goodIcon.gameObject.SetActive(false);
            }
            else
            {
                goodIcon.gameObject.SetActive(true);
                goodIcon.sprite = ConfigurationManager.Instance.GoodsConfig.ConfigData[good!.Value].Icon;
            }
        }

        public void SetAmount(int amount)
        {
            if (amount <= 0)
            {
                amountText.gameObject.SetActive(false);
                disabledIcon.gameObject.SetActive(true);
            }
            else
            {
                amountText.gameObject.SetActive(_showAmountText);
                disabledIcon.gameObject.SetActive(false);
                amountText.text = amount.ToString();
            }
        }

        public void ShowText(bool show)
        {
            _showAmountText = show;
            amountText.gameObject.SetActive(_showAmountText);
        }
        
        public void SetIsProduced(bool isProduced)
        {
            productionIcon.gameObject.SetActive(isProduced);
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                Clicked?.Invoke();
            }
        }
    }
}