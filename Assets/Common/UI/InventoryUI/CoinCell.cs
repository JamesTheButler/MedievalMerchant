using System;
using Common.Utility;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace Common.UI.InventoryUI
{
    public sealed class CoinCell : MonoBehaviour, IPointerClickHandler
    {
        public event Action Clicked;

        [SerializeField, Required]
        private Image icon, completedIcon;

        [SerializeField, Required]
        private TMP_Text amountText, amountTextSecondary;

        public void SetAmount(int amount)
        {
            var amountString = amount.ToString();
            amountText?.SetText(amountString);
            amountTextSecondary?.SetText(amountString);

            var isCompleted = amount <= 0;

            amountText?.gameObject.SetActive(!isCompleted);
            completedIcon.gameObject.SetActive(isCompleted);
            icon.color = icon.color.WithAlpha(isCompleted ? 0.25f : 1f);
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
