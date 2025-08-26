using System;
using Data;
using Data.Setup;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace UI
{
    public sealed class BuySellPopup : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<Good, TradeType> tradeInitiated;

        [SerializeField]
        private TMP_Text goodNameText;

        [SerializeField]
        private Button buyButton;

        [SerializeField]
        private Button sellButton;

        [FormerlySerializedAs("supplyDemandIcon"),SerializeField]
        private Image marketStateIcon;

        [FormerlySerializedAs("supplyDemandText"),SerializeField]
        private TMP_Text marketStateText;

        private readonly Lazy<MarketStateIcons> _marketStateIcons = new(() => SetupManager.Instance.MarketStateIcons);

        private Good _good;
        private MarketState? _marketState;

        private void Start()
        {
            buyButton.onClick.AddListener(() => TradeInitiated(TradeType.Buy));
            sellButton.onClick.AddListener(() => TradeInitiated(TradeType.Sell));
        }

        private void TradeInitiated(TradeType tradeType)
        {
            Hide();
            tradeInitiated?.Invoke(_good, tradeType);
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        public void SetGood(Good good)
        {
            _good = good;
            goodNameText.text = SetupManager.Instance.GoodInfoManager.GoodInfos[good].GoodName;
        }

        public void CanBuy(bool canBuy)
        {
            buyButton.interactable = canBuy;
        }

        public void CanSell(bool canSell)
        {
            sellButton.interactable = canSell;
        }

        public void SetMarketState(MarketState marketState)
        {
            if (_marketState == marketState) return;

            marketStateIcon.sprite = _marketStateIcons.Value.Icons[marketState];
            marketStateText.text = marketState.ToString();

            _marketState = marketState;
        }
    }
}