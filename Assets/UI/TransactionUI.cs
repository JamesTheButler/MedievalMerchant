using System;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TransactionUI : MonoBehaviour
    {
        public event Action TransactionAborted;
        public event Action<Good, int, int, TransactionType> TransactionConfirmed;

        [SerializeField]
        private TMP_Text goodAmountText;

        [SerializeField]
        private Image goodIcon;

        [SerializeField]
        private TMP_Text coinAmountText;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Button sellButton;

        [SerializeField]
        private Button cancelButton;

        [SerializeField]
        private Slider amountSlider;

        private GoodInfoManager _goodInfoManager;
        private GoodInfo _goodInfo;
        private Good _good;
        private TransactionType _transactionType;
        private int _unitPrice;
        private int _amount;
        private int _totalPrice;

        private void Start()
        {
            _goodInfoManager = Setup.Instance.GoodInfoManager;
            gameObject.SetActive(false);
        }

        public void Initialize(Good good, TransactionType transactionType, int maxAmount)
        {
            _transactionType = transactionType;
            _goodInfo = _goodInfoManager.GoodInfos[good];
            _unitPrice = (int)_goodInfo.BasePrice;
            goodIcon.sprite = _goodInfo.Icon;

            // buttons
            buyButton.gameObject.SetActive(transactionType == TransactionType.Buy);
            sellButton.gameObject.SetActive(transactionType == TransactionType.Sell);
            buyButton.onClick.AddListener(() =>
            {
                TransactionConfirmed?.Invoke(_good, _amount, _totalPrice, TransactionType.Buy);
            });
            sellButton.onClick.AddListener(() =>
            {
                TransactionConfirmed?.Invoke(_good, _amount, _totalPrice, TransactionType.Sell);
            });
            cancelButton.onClick.AddListener(() => TransactionAborted?.Invoke());

            amountSlider.minValue = 0;
            amountSlider.maxValue = maxAmount;
            amountSlider.onValueChanged.AddListener(amount => SetAmount((int)amount));

            gameObject.SetActive(true);
        }

        private void SetAmount(int amount)
        {
            _amount = amount;
            goodAmountText.text = $"x{amount}";
            _totalPrice = amount * _unitPrice;
            var minus = _transactionType == TransactionType.Buy ? "-" : "";
            coinAmountText.text = $"{minus}{_totalPrice}";
        }
    }
}