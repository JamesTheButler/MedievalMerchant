using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.UI.Elements.Cells;
using Common.UI.Elements.Panels;
using Common.UI.Tooltips;
using Common.UI.Utility;
using Common.Utility;
using Features.Levels.FeatureFlags;
using Features.Localization.Data;
using Features.Towns;
using Features.Towns.Flags.UI;
using Features.Towns.Missions;
using Features.Trade.Haggling;
using Features.Trade.Haggling.UI;
using Features.Trade.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Trade.UI
{
    public sealed class TradeUI : DynamicPanel
    {
        [field: SerializeField, Required]
        public Button TradeButton { get; private set; }

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
        private HaggleGroup haggleGroup;

        [SerializeField, Required]
        private Button cancelButton, quickButtonMax, quickButtonMission, quickButton15, quickButton30;

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

        [SerializeField, Required]
        private CanvasGroup profitGroup;

        private readonly Bindings _bindings = new();

        // set up on Initialize
        private GameplayModel _model;
        private TradeService _tradeService;
        private Selection _selection;
        private TradeLocalizationResources _loc;

        // set up on SetUp (i.e. needs to be cleared in TearDown)
        private Town _town;
        private Good _good;
        private TradeType _tradeType;
        private Inventory.Inventory _buyingInventory, _sellingInventory;
        private OngoingTrade _ongoingTrade;
        private TradeValidator _tradeValidator;

        private bool _isStuckToMax;
        private bool _wasSuccessfulTrade;
        private bool _isHagglingEnabled;

        private const HaggleLevel InitialHaggleLevel = HaggleLevel.Fair;

        public override void Initialize()
        {
            _model = GameplayContext.Instance.Model;
            _tradeService = GameplayContext.Instance.Services.TradeService;
            _selection = GameplayContext.Instance.Selection;
            _loc = ResourceManager.Instance.LocalizationResources.Trade;

            _isHagglingEnabled = GameplayContext.Instance.LevelInfo.HasFeature(LevelFeatureFlags.Haggling);

            TradeButton.onClick.AddListener(CompleteTrade);
            cancelButton.onClick.AddListener(AbortTrade);
            quickButtonMax.onClick.AddListener(SetMaxAmount);
            quickButtonMission.onClick.AddListener(SetActiveMissionAmount);
            quickButton15.onClick.AddListener(() => SetSliderValue(15));
            quickButton30.onClick.AddListener(() => SetSliderValue(30));

            if (_isHagglingEnabled)
            {
                haggleGroup.HaggleLevelChanged += OnSelectedHaggleLevelChanged;
            }
            else
            {
                Destroy(haggleGroup.gameObject);
            }

            SetUpSlider();
        }

        public void SetUp(Good good, TradeType tradeType)
        {
            _good = good;
            _tradeType = tradeType;
        }

        public void CompleteTrade()
        {
            if (_ongoingTrade.Amount <= 0 || _ongoingTrade.Amount > GetMaxAffordableAmount())
                return;

            var tradeValidationResult = _tradeValidator.Validate(_tradeType, _good, _ongoingTrade.Amount);
            if (!tradeValidationResult.Success)
            {
                Debug.LogError($"Attempted to complete invalid trade.: {tradeValidationResult.Error}");
                //TODO: this should probably be reported to the user via popup
                return;
            }

            _ongoingTrade.Complete();
            _wasSuccessfulTrade = true;
            Close();
        }

        protected override void OnOpen()
        {
            _town = _selection.SelectedTown;
            _ongoingTrade = _tradeService.InitializeTrade(_town, _good, _tradeType);
            _tradeValidator = new TradeValidator(_model.Player, _town);

            if (_isHagglingEnabled)
            {
                haggleGroup.SetUp(InitialHaggleLevel, _tradeType);
            }

            _ongoingTrade.SetHaggleLevel(InitialHaggleLevel);

            _bindings.Track(
                _ongoingTrade.TotalPrice.Observe(OnTotalPriceChanged),
                _ongoingTrade.ReputationChange.Observe(RefreshTownReputationText, false),
                _ongoingTrade.Profit.Observe(RefreshProfitText),
                _town.ReputationModel.Reputation.Observe(RefreshTownReputationText, true),
                _town.Inventory.Funds.Observe(RefreshTownFundsText, true),
                _model.Player.Inventory.Funds.Observe(RefreshPlayerFundsText, true),
                _town.Missions.MissionAdded.Observe(OnMissionAdded)
            );

            RefreshMissionAmountButton();

            var tradeTypeString = _tradeType == TradeType.Buy ? _loc.BuyString : _loc.SellString;
            TradeButton.GetText().text = tradeTypeString.GetLocalizedString();
            townNameText.text = _town.Name;
            townFlagRenderer.SetFlag(_town.FlagInfo);

            goodCell.SetGood(_good);
            goodDirectionIcon.sprite = _tradeType == TradeType.Buy ? playerGetsIcon : playerGivesIcon;
            coinDirectionIcon.sprite = _tradeType == TradeType.Sell ? playerGetsIcon : playerGivesIcon;

            SetUpInventories();

            priceTooltip.SetData(_ongoingTrade.SinglePrice);

            amountSlider.value = 0;
            SetMaxAmount();

            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            _bindings.UnbindAll();
            gameObject.SetActive(false);

            priceTooltip.SetData(null);
            _buyingInventory = null;
            _sellingInventory = null;

            if (!_wasSuccessfulTrade)
            {
                _ongoingTrade.Abort();
            }

            _wasSuccessfulTrade = false;
        }

        private void SetSliderValue(int value)
        {
            amountSlider.value = Mathf.Min(value, amountSlider.maxValue);
        }

        private void OnSelectedHaggleLevelChanged(HaggleLevel level)
        {
            _ongoingTrade.SetHaggleLevel(level);
        }

        private void SetMaxAmount()
        {
            var maxAffordableGoodAmount = GetMaxAffordableAmount();
            SetSliderValue(maxAffordableGoodAmount);
        }

        private int GetMaxAffordableAmount()
        {
            return Mathf.FloorToInt(_buyingInventory.Funds.Value / _ongoingTrade.SinglePrice);
        }

        private void SetUpSlider()
        {
            amountSlider.minValue = 0;
            amountSlider.value = 0;
            amountSlider.onValueChanged.AddListener(TradeSliderUpdate);
        }

        private void TradeSliderUpdate(float amount)
        {
            _isStuckToMax = Mathf.Approximately(amount, amountSlider.maxValue);

            var intAmount = (int)amount;
            sliderValueText.text = intAmount.ToString("0");
            _ongoingTrade.SetAmount(intAmount);
            RefreshGoodAmountText(intAmount);
            RefreshTradeButtonState();
        }

        private void OnMissionAdded(Mission mission)
        {
            if (mission.Good != _good)
                return;

            RefreshMissionAmountButton();
        }

        private void SetUpInventories()
        {
            var player = _model.Player.Inventory;
            var townInventory = _town.Inventory;

            _buyingInventory = _tradeType == TradeType.Buy ? player : townInventory;
            _sellingInventory = _tradeType == TradeType.Sell ? player : townInventory;

            _bindings.Track(
                _sellingInventory.GoodAmountChanged.Observe(OnSellingInventoryGoodUpdated),
                _buyingInventory.Funds.Observe(RefreshTradeButtonState, true)
            );
            OnSellingInventoryGoodUpdated(_good, _sellingInventory.Goods[_good]);
        }

        private void SetActiveMissionAmount()
        {
            var hasMission = _town.Missions.Missions.TryGetValue(_good, out var mission);
            if (!hasMission)
                return;

            SetSliderValue(mission.RemainingCount);
        }

        private void OnSellingInventoryGoodUpdated(Good good, int amount)
        {
            if (good != _good)
                return;

            amountSlider.maxValue = amount;
            if (_isStuckToMax)
            {
                amountSlider.value = amount;
            }

            RefreshTradeButtonState();
        }

        private void AbortTrade()
        {
            _ongoingTrade.Abort();
            _wasSuccessfulTrade = false;
            Close();
        }

        # region Prices

        private void OnTotalPriceChanged(float totalPrice)
        {
            RefreshTotalPriceText();
            RefreshTownFundsText();
            RefreshPlayerFundsText();
        }

        private void RefreshTotalPriceText()
        {
            var totalPrice = _ongoingTrade.TotalPrice.Value;
            var price = $"{totalPrice:0.##}";
            if (_tradeType == TradeType.Buy && !totalPrice.IsApproximately(0f))
            {
                price = "-" + price;
            }

            coinAmountText.text = price;
        }

        private void RefreshTownFundsText()
        {
            var fundsChange = _ongoingTrade.TotalPrice.Value;
            var fundsChangeText = _tradeType == TradeType.Buy
                ? $"+{fundsChange:0.#}".WithStyle(Style.Good)
                : $"-{fundsChange:0.#}".WithStyle(Style.Bad);

            var dataObject = new
            {
                _int_Current = _town.Inventory.Funds.Value,
                Change = fundsChangeText,
            };


            townFundsText.text = _loc.FundsSummary.GetLocalizedString(dataObject);
        }

        private void RefreshPlayerFundsText()
        {
            var fundsChange = _ongoingTrade.TotalPrice.Value;
            var fundsChangeText = _tradeType == TradeType.Sell
                ? $"+{fundsChange:0.#}".WithStyle(Style.Good)
                : $"-{fundsChange:0.#}".WithStyle(Style.Bad);

            var dataObject = new
            {
                _int_Current = _model.Player.Inventory.Funds.Value,
                Change = fundsChangeText,
            };

            playerFundsText.text = _loc.FundsSummary.GetLocalizedString(dataObject);
        }

        private void RefreshProfitText(float? profit)
        {
            var isProfitShowable = _tradeType == TradeType.Sell && profit != null && float.IsFinite(profit.Value);

            profitGroup.alpha = isProfitShowable ? 1f : 0f;

            if (!isProfitShowable)
            {
                lossProfitText.text = string.Empty;
                return;
            }

            var netString = profit.Value < 0 ? _loc.NetLossString : _loc.NetProfitString;
            var netObject = new { _int_Amount = profit.Value };
            lossProfitText.text = netString.GetLocalizedString(netObject);
        }

        # endregion Prices

        private void RefreshGoodAmountText(int amount)
        {
            goodAmountText.text = $"x{amount}";
        }

        private void RefreshMissionAmountButton()
        {
            quickButtonMission.gameObject.SetActive(_town.Missions.Missions.ContainsKey(_good));
        }

        private void RefreshTradeButtonState()
        {
            var totalPrice = _ongoingTrade.TotalPrice;
            var buyerFunds = _buyingInventory.Funds;

            var canAfford = buyerFunds >= totalPrice;
            var isTradePossible = _ongoingTrade.Amount > 0 && canAfford;
            TradeButton.interactable = isTradePossible;
            tradeButtonTooltip.SetEnabled(!canAfford);

            if (isTradePossible)
                return;

            var notEnoughCoinMessage = _tradeType == TradeType.Buy
                ? _loc.YouNotEnoughCoin.GetLocalizedString()
                : _loc.TownNotEnoughCoin.GetLocalizedString(new { TownName = _town.Name });

            tradeButtonTooltip.SetData(notEnoughCoinMessage);
        }

        private void RefreshTownReputationText()
        {
            var currentRep = _town.ReputationModel.Reputation.Value;
            var repChange = _ongoingTrade.ReputationChange.Value;
            var repChangeText = $"{repChange.Sign(false)}{repChange:0.#}".WithStyle(repChange.GetNumberStyle());

            var dataObject = new
            {
                _int_Current = currentRep,
                Change = repChangeText,
            };

            townReputationText.text = _loc.ReputationSummary.GetLocalizedString(dataObject);
        }
    }
}