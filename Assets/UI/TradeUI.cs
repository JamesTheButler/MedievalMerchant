using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class TradeUI : MonoBehaviour
    {
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

        private bool _isInitialized;
        
        private GoodInfoManager _goodInfoManager;
        private GoodInfo _goodInfo;
        private Good _good;
        private TradeType _tradeType;
        private int _unitPrice;

        private int _sellerGoodAmount;
        private int _buyerFunds;

        private int _tradeAmount;
        private int _totalPrice;

        private Button _activeButton;

        private Inventory _buyingInventory;
        private Inventory _sellingInventory;

        private readonly Color _coinTextColorGood = Color.green;
        private readonly Color _coinTextColorBad = Color.red;

        private void Start()
        {
            gameObject.SetActive(false);
        }

        public void Initialize(Good good, TradeType tradeType)
        {
            _tradeType = tradeType;
            _good = good;
            _goodInfo = Setup.Instance.GoodInfoManager.GoodInfos[good];
            _unitPrice = (int)_goodInfo.BasePrice;
            goodIcon.sprite = _goodInfo.Icon;

            SetUpInventories();
            SetUpButtons();

            SetAmount(0);
            amountSlider.minValue = 0;
            amountSlider.value = 0;
            amountSlider.maxValue = _sellerGoodAmount;
            amountSlider.onValueChanged.AddListener(TradeSliderUpdate);

            gameObject.SetActive(true);

            _isInitialized = true;
        }

        public void Reset()
        {
            if (!_isInitialized) return;
            amountSlider.onValueChanged.RemoveListener(TradeSliderUpdate);
            _sellingInventory.GoodUpdated -= OnSellingInventoryGoodUpdated;
            _buyingInventory.FundsUpdated -= OnBuyingInventoryFundsUpdated;
            _isInitialized = false;
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
        }

        private void SetUpInventories()
        {
            var player = Model.Instance.Player.Inventory;
            var town = Selection.Instance.SelectedTown?.Inventory;

            if (town is null) return;

            _buyingInventory = _tradeType == TradeType.Buy ? player : town;
            _sellingInventory = _tradeType == TradeType.Sell ? player : town;

            _sellingInventory.GoodUpdated += OnSellingInventoryGoodUpdated;
            _buyingInventory.FundsUpdated += OnBuyingInventoryFundsUpdated;

            _buyerFunds = _buyingInventory.Funds;
            _sellerGoodAmount = _sellingInventory.Get(_good);
        }

        private void OnBuyingInventoryFundsUpdated(int newFunds)
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
        }

        private void AbortTrade()
        {
            gameObject.SetActive(false);
        }

        private void CompleteTrade()
        {
            _buyingInventory.RemoveFunds(_totalPrice);
            _buyingInventory.AddGood(_good, _tradeAmount);

            _sellingInventory.AddFunds(_totalPrice);
            _sellingInventory.RemoveGood(_good, _tradeAmount);

            gameObject.SetActive(false);
        }

        private void SetAmount(int amount)
        {
            _tradeAmount = amount;
            _totalPrice = amount * _unitPrice;

            goodAmountText.text = $"x{amount}";

            var priceText = $"{_totalPrice}";

            if (_tradeType == TradeType.Buy && amount > 0)
            {
                priceText = "-" + priceText;
            }

            coinAmountText.text = priceText;
        }

        private void EvaluateTotalPrice()
        {
            var isTradePossible = _buyerFunds > _totalPrice;

            _activeButton.interactable = isTradePossible;
            coinAmountText.color = isTradePossible ? _coinTextColorBad : _coinTextColorGood;
        }
    }
}