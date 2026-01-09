using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Modifiable;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Common.UI.Utility;
using Common.Utility;
using Features.Player.Logic;
using Features.Towns;
using Features.Towns.Flags.UI;
using Features.Towns.Missions;
using Features.Trade.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Trade.UI
{
    public sealed class TradeUI : InitializableBehavior
    {
        [SerializeField, Required]
        private TMP_Text
            goodAmountText,
            coinAmountText,
            townNameText,
            lossProfitText,
            playerFundsText,
            townFundsText,
            townReputationText,
            sliderValueText;

        [SerializeField, Required]
        private GoodCell goodCell;

        [SerializeField, Required]
        private Button tradeButton, cancelButton, maxAmountButton, missionAmountButton;

        [SerializeField, Required]
        private SimpleTooltipHandler tradeButtonTooltip;

        [SerializeField, Required]
        private Image goodDirectionIcon, coinDirectionIcon;

        [SerializeField, Required]
        private Sprite playerGetsIcon, playerGivesIcon;

        [SerializeField, Required]
        private Slider amountSlider;

        [SerializeField, Required]
        private FlagRenderer townFlagRenderer;

        [SerializeField, Required]
        private ModifiableTooltipHandler priceTooltip;

        private GameplayModel _model;
        private TradeService _tradeService;
        private Selection _selection;
        private TradeTracker _tradeTracker;

        private bool _isInitialized;

        private Town _town;
        private Good _good;
        private TradeType _tradeType;

        private float _buyerFunds;
        private int _tradeAmount;
        private float _totalPrice;
        private float _singlePrice;

        private Inventory.Inventory _buyingInventory;
        private Inventory.Inventory _sellingInventory;

        private PriceManager _priceManager;
        private ModifiableVariable _observedPrice;

        private const string NetProfitStringFormat = "You will be making a profit of {0} with this trade.";
        private const string NetLossStringFormat = "You will be making a loss of {0} with this trade.";

        public override void Initialize()
        {
            tradeButton.onClick.AddListener(CompleteTrade);
            cancelButton.onClick.AddListener(AbortTrade);
            maxAmountButton.onClick.AddListener(SetMaxAmount);
            missionAmountButton.onClick.AddListener(SetActiveMissionAmount);

            SetUpSlider();

            _model = GameplayContext.Instance.Model;
            _tradeTracker = _model.Player.TradeTracker;
            _tradeService = GameplayContext.Instance.Services.TradeService;
            _selection = GameplayContext.Instance.Selection;
        }

        public void Show(Good good, TradeType tradeType)
        {
            _good = good;
            _tradeType = tradeType;
            _town = _selection.SelectedTown;
            _town.ReputationManager.Reputation.Observe(RefreshTownReputationText);
            RefreshTownReputationText();
            _town.Inventory.Funds.Observe(RefreshTownFundsText);
            _town.Missions.MissionAdded += OnMissionAdded;
            RefreshMissionAmountButton();

            _priceManager = _town.PriceManager;
            _model.Player.Inventory.Funds.Observe(RefreshPlayerFundsText);

            tradeButton.GetText().text = tradeType == TradeType.Buy ? "Buy" : "Sell";

            townNameText.text = _town.Name;
            townFlagRenderer.SetFlag(_town.FlagInfo);

            goodCell.SetGood(_good);
            goodDirectionIcon.sprite = tradeType == TradeType.Buy ? playerGetsIcon : playerGivesIcon;
            coinDirectionIcon.sprite = tradeType == TradeType.Sell ? playerGetsIcon : playerGivesIcon;

            SetUpInventories();

            _observedPrice = _priceManager.GetPrice(good, tradeType);
            _observedPrice.Observe(OnGoodPriceChanged);
            priceTooltip.SetData(_observedPrice);

            gameObject.SetActive(true);

            SetMaxAmount();

            _isInitialized = true;
        }

        private void OnMissionAdded(Mission mission)
        {
            if (mission.Good != _good)
                return;

            RefreshMissionAmountButton();
        }

        public void Hide()
        {
            gameObject.SetActive(false);

            if (!_isInitialized) return;

            _model.Player.Inventory.Funds.StopObserving(RefreshPlayerFundsText);
            _town.ReputationManager.Reputation.StopObserving(RefreshTownReputationText);
            _town.Inventory.Funds.StopObserving(RefreshTownFundsText);
            _sellingInventory.GoodUpdated -= OnSellingInventoryGoodUpdated;
            _town.Missions.MissionAdded -= OnMissionAdded;
            _buyingInventory.Funds.StopObserving(OnBuyingInventoryFundsUpdated);

            priceTooltip.SetData(null);
            _observedPrice.StopObserving(OnGoodPriceChanged);
            _observedPrice = null;
            _buyingInventory = null;
            _sellingInventory = null;

            _isInitialized = false;
        }

        private void SetMaxAmount()
        {
            var maxAffordableGoodAmount = Mathf.FloorToInt(_buyingInventory.Funds.Value / _singlePrice);
            amountSlider.value = Mathf.Min(maxAffordableGoodAmount, amountSlider.maxValue);
            RefreshTotalPrice();
        }

        private void OnGoodPriceChanged(float newPrice)
        {
            _singlePrice = newPrice;
            RefreshTotalPrice();
        }

        private void SetUpSlider()
        {
            amountSlider.minValue = 0;
            amountSlider.value = 0;
            amountSlider.onValueChanged.AddListener(TradeSliderUpdate);
        }

        private void TradeSliderUpdate(float amount)
        {
            sliderValueText.text = amount.ToString("0");
            SetAmount((int)amount);
            RefreshButtonState();
        }

        private void SetUpInventories()
        {
            var player = _model.Player.Inventory;
            var townInventory = _town.Inventory;

            _buyingInventory = _tradeType == TradeType.Buy ? player : townInventory;
            _sellingInventory = _tradeType == TradeType.Sell ? player : townInventory;

            _sellingInventory.GoodUpdated += OnSellingInventoryGoodUpdated;
            OnSellingInventoryGoodUpdated(_good, _sellingInventory.Goods[_good]);
            _buyingInventory.Funds.Observe(OnBuyingInventoryFundsUpdated);
        }

        private void SetActiveMissionAmount()
        {
            var hasMission = _town.Missions.Missions.TryGetValue(_good, out var mission);
            if (!hasMission)
                return;

            amountSlider.value = mission.RemainingCount;
        }

        private void OnBuyingInventoryFundsUpdated(float newFunds)
        {
            _buyerFunds = newFunds;
            RefreshButtonState();
        }

        private void OnSellingInventoryGoodUpdated(Good good, int amount)
        {
            if (good != _good)
                return;

            amountSlider.maxValue = amount;
            RefreshTotalPrice();
            RefreshButtonState();
        }

        private void AbortTrade()
        {
            Hide();
        }

        private void CompleteTrade()
        {
            var tradeInfo = new TradeInfo(_town, _tradeType, _good, _tradeAmount, _totalPrice, 1);

            _buyingInventory.RemoveFunds(_totalPrice);
            _sellingInventory.AddFunds(_totalPrice);

            _buyingInventory.AddGood(_good, _tradeAmount);
            _sellingInventory.RemoveGood(_good, _tradeAmount);
            // this should replace the line above
            _tradeService.CompleteTrade(tradeInfo);

            Hide();
        }

        private void SetAmount(int amount)
        {
            _tradeAmount = amount;
            goodAmountText.text = $"x{_tradeAmount}";
            RefreshTotalPrice();
        }

        private void RefreshMissionAmountButton()
        {
            missionAmountButton.gameObject.SetActive(_town.Missions.Missions.ContainsKey(_good));
        }

        private void RefreshTotalPrice()
        {
            _totalPrice = _tradeAmount * _singlePrice;

            RefreshTotalPriceText();
            RefreshFundsChangeTexts();
            RefreshTownFundsText();
            RefreshPlayerFundsText();

            lossProfitText.gameObject.SetActive(_tradeType == TradeType.Sell);
            if (_tradeType == TradeType.Sell)
            {
                RefreshLossOrProfitText();
            }
        }

        private void RefreshFundsChangeTexts()
        {
            var fundsGainedText = $"{_totalPrice:0.#}".WithStyle(Style.Good);
            var fundsLostText = $"-{_totalPrice:0.#}".WithStyle(Style.Bad);
            var playerChangeText = _tradeType == TradeType.Sell ? fundsGainedText : fundsLostText;

            playerFundsText.text = $"Funds: {_model.Player.Inventory.Funds.Value:0.#} ({playerChangeText})";
        }

        private void RefreshTotalPriceText()
        {
            var price = $"{_totalPrice:0.##}";
            if (_tradeType == TradeType.Buy && _tradeAmount > 0)
            {
                price = "-" + price;
            }

            coinAmountText.text = price;
        }

        private void RefreshLossOrProfitText()
        {
            var trackedInfo = _tradeTracker.TrackedGoods.GetValueOrDefault(_good);
            if (trackedInfo == null)
            {
                Debug.LogWarning($"TradeTracker did not have entry for {_good}. Something's wrong.");
                lossProfitText.gameObject.SetActive(false);
                return;
            }

            if (_tradeAmount <= 0)
            {
                lossProfitText.gameObject.SetActive(false);
                return;
            }

            // diff between what the player bought the goods for and what they're selling it for
            var difference = _totalPrice - trackedInfo.AveragePrice * _tradeAmount;
            var style = difference.GetNumberStyle();
            var differenceText = $"{difference.Sign()}{difference:0.##} coin".WithStyle(style);
            var formatter = difference < 0 ? NetLossStringFormat : NetProfitStringFormat;
            var lossOrProfitMessage = string.Format(formatter, differenceText);
            lossProfitText.text = lossOrProfitMessage;
        }

        private void RefreshButtonState()
        {
            var isTradePossible = _buyerFunds >= _totalPrice;
            tradeButton.interactable = isTradePossible;
            tradeButtonTooltip.SetEnabled(!isTradePossible);

            if (isTradePossible)
                return;

            var notEnoughCoinMessage = _tradeType == TradeType.Buy
                ? "You do not have enough coin."
                : $"{_town.Name} does not have enough coin.";

            tradeButtonTooltip.SetData(notEnoughCoinMessage);
        }

        private void RefreshTownReputationText()
        {
            townReputationText.text = $"Reputation: {_town.ReputationManager.Reputation.Value:0.#}";
        }

        private void RefreshTownFundsText()
        {
            var townChangeText = _tradeType == TradeType.Buy
                ? $"{_totalPrice:0.#}".WithStyle(Style.Good)
                : $"-{_totalPrice:0.#}".WithStyle(Style.Bad);
            townFundsText.text = $"Funds: {_town.Inventory.Funds.Value:0.#} ({townChangeText})";
        }

        private void RefreshPlayerFundsText()
        {
            var townChangeText = _tradeType == TradeType.Sell
                ? $"{_totalPrice:0.#}".WithStyle(Style.Good)
                : $"-{_totalPrice:0.#}".WithStyle(Style.Bad);
            playerFundsText.text = $"Funds: {_model.Player.Inventory.Funds.Value:0.#} ({townChangeText})";
        }
    }
}