using System;
using Common;
using Common.Config;
using Common.Types;
using Features.Goods.Config;
using Features.Towns;
using Features.Trade.Logic.Price;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Trade.UI
{
    public sealed class TradeUI : MonoBehaviour
    {
        [SerializeField, Required]
        private TMP_Text goodAmountText;

        [SerializeField, Required]
        private Image goodIcon;

        [SerializeField, Required]
        private TMP_Text coinAmountText;

        [SerializeField, Required]
        private Button buyButton;

        [SerializeField, Required]
        private Button sellButton;

        [SerializeField, Required]
        private Button cancelButton;

        [SerializeField, Required]
        private Slider amountSlider;

        [SerializeField, Required]
        private TMP_Text modifiersText;

        private readonly Lazy<Model> _model = new(() => Model.Instance);
        private readonly Lazy<Selection> _selection = new(() => Selection.Instance);
        private readonly Lazy<Colors> _colors = new(() => ConfigurationManager.Instance.Colors);
        private readonly Lazy<GoodsConfig> _configurationManager = new(() => ConfigurationManager.Instance.GoodsConfig);

        private bool _isInitialized;

        private GoodConfigData _goodConfigData;
        private Good _good;
        private TradeType _tradeType;
        private int _sellerGoodAmount;
        private float _buyerFunds;
        private int _tradeAmount;
        private float _totalPrice;

        private Button _activeButton;

        private Inventory.Inventory _buyingInventory;
        private Inventory.Inventory _sellingInventory;

        private PriceCalculator _priceCalculator;

        public void Initialize(Good good, TradeType tradeType)
        {
            _tradeType = tradeType;
            _good = good;
            _goodConfigData = _configurationManager.Value.ConfigData[good];
            goodIcon.sprite = _goodConfigData.Icon;

            SetUpButtons();
            SetUpInventories();

            SetAmount(0);
            amountSlider.minValue = 0;
            amountSlider.value = 0;
            amountSlider.maxValue = _sellerGoodAmount;
            amountSlider.onValueChanged.AddListener(TradeSliderUpdate);

            gameObject.SetActive(true);

            _isInitialized = true;
        }

        public void Hide()
        {
            if (!_isInitialized) return;

            amountSlider.onValueChanged.RemoveListener(TradeSliderUpdate);
            _sellingInventory.GoodUpdated -= OnSellingInventoryGoodUpdated;
            _buyingInventory.Funds.StopObserving(OnBuyingInventoryFundsUpdated);

            _activeButton.onClick.RemoveAllListeners();
            cancelButton.onClick.RemoveAllListeners();

            _priceCalculator = null;
            _buyingInventory = null;
            _sellingInventory = null;

            _isInitialized = false;

            gameObject.SetActive(false);
        }

        private void TradeSliderUpdate(float amount)
        {
            SetAmount((int)amount);
            EvaluateTotalPrice();
        }

        private void SetUpButtons()
        {
            _activeButton = _tradeType == TradeType.Buy ? buyButton : sellButton;
            _activeButton.gameObject.SetActive(true);
            _activeButton.onClick.AddListener(CompleteTrade);
            cancelButton.onClick.AddListener(AbortTrade);

            var inactiveButton = _tradeType == TradeType.Buy ? sellButton : buyButton;
            inactiveButton.gameObject.SetActive(false);
        }

        private void SetUpInventories()
        {
            var player = _model.Value.Player.Inventory;
            var town = _selection.Value.SelectedTown;

            if (town is null) return;

            var townInventory = town.Inventory;
            _priceCalculator = new PriceCalculator(town);

            _buyingInventory = _tradeType == TradeType.Buy ? player : townInventory;
            _sellingInventory = _tradeType == TradeType.Sell ? player : townInventory;

            _sellingInventory.GoodUpdated += OnSellingInventoryGoodUpdated;
            _buyingInventory.Funds.Observe(OnBuyingInventoryFundsUpdated);

            _sellerGoodAmount = _sellingInventory.Get(_good);
        }

        private void OnBuyingInventoryFundsUpdated(float newFunds)
        {
            _buyerFunds = newFunds;
            EvaluateTotalPrice();
        }

        private void OnSellingInventoryGoodUpdated(Good good, int amount)
        {
            if (good != _good)
                return;

            _sellerGoodAmount = amount;
            amountSlider.maxValue = amount;
            UpdatePrice();
            EvaluateTotalPrice();
        }

        private void AbortTrade()
        {
            Hide();
        }

        private void CompleteTrade()
        {
            _buyingInventory.RemoveFunds(_totalPrice);
            _buyingInventory.AddGood(_good, _tradeAmount);

            _sellingInventory.AddFunds(_totalPrice);
            _sellingInventory.RemoveGood(_good, _tradeAmount);

            Hide();
        }

        private void SetAmount(int amount)
        {
            _tradeAmount = amount;
            goodAmountText.text = $"x{_tradeAmount}";

            UpdatePrice();
        }

        private void UpdatePrice()
        {
            var goodPrice = _priceCalculator.GetPrice(_good, _tradeType);
            _totalPrice = _tradeAmount * goodPrice.Value;

            var priceText = $"{_totalPrice:N2}";

            if (_tradeType == TradeType.Buy && _tradeAmount > 0)
            {
                priceText = "-" + priceText;
            }

            coinAmountText.text = priceText;
            modifiersText.text = goodPrice.ToString();
        }

        private void EvaluateTotalPrice()
        {
            var isTradePossible = _buyerFunds >= _totalPrice;

            _activeButton.interactable = isTradePossible;
            coinAmountText.color = isTradePossible ? _colors.Value.FontDark : _colors.Value.Bad;
        }
    }
}