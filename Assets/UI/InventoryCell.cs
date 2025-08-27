using System;
using Data;
using Data.Configuration;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI
{
    public sealed class InventoryCell : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        public Good Good { get; private set; }

        [SerializeField]
        private Image goodIcon;

        [SerializeField]
        private Image disabledIcon;

        [SerializeField]
        private Image productionIcon;

        [SerializeField]
        private TMP_Text amountText;

        public void SetGood(Good good)
        {
            Good = good;
            goodIcon.sprite = ConfigurationManager.Instance.GoodsConfig.ConfigData[good].Icon;
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
                amountText.gameObject.SetActive(true);
                disabledIcon.gameObject.SetActive(false);
                amountText.text = amount.ToString();
            }
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