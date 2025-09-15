using System;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.Popups
{
    public sealed class PaymentPopup : Popup
    {
        [SerializeField, Required]
        private Button button;

        private TMP_Text _text;

        private void Start()
        {
            _text = button.GetComponent<TMP_Text>();
        }

        public void Setup(int price, Action onClick)
        {
            _text.text = price.ToString("N0");

            button.onClick.RemoveAllListeners();
            button.onClick.AddListener(() => onClick?.Invoke());
        }
    }
}