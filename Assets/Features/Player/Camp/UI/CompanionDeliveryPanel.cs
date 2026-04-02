using Common.Infrastructure.Gameplay;
using Common.UI.Elements.Cells;
using Common.UI.Elements.Panels;
using Common.UI.InventoryUI;
using Common.UI.Tooltips;
using Features.Player.Logic;
using Features.Player.Retinue;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Player.Camp.UI
{
    public sealed class CompanionDeliveryPanel : DynamicPanel
    {
        [SerializeField, Required]
        private Slider amountSlider;

        [SerializeField, Required]
        private TMP_Text sliderValueText, coinSubstituteText;

        [SerializeField, Required]
        private InventoryCell goodCell;

        [SerializeField, Required]
        private CoinCell coinCell;

        [SerializeField, Required]
        private Button deliverButton, coinSubstituteButton;

        [SerializeField, Required]
        private SimpleTooltipHandler deliverTooltip, coinSubstituteTooltip;

        [SerializeField]
        private LocalizedString notEnoughCoinString, notEnoughGoodString;

        private PlayerModel _playerModel;
        private RetinueModel _retinueModel;
        private CompanionDeliveryService _deliveryService;

        private CompanionType _companionType;
        private CompanionMissionItem _missionItem;

        protected override void OnInitialize()
        {
            _playerModel = GameplayContext.Instance.Model.Player;
            _retinueModel = _playerModel.RetinueModel;
            _deliveryService = GameplayContext.Instance.Services.CompanionDeliveryService;

            deliverButton.onClick.AddListener(OnDeliverClicked);
            coinSubstituteButton.onClick.AddListener(OnSubstituteClicked);
            amountSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void SetUp(CompanionType companionType, CompanionMissionItem missionItem)
        {
            _companionType = companionType;
            _missionItem = missionItem;
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);

            var mission = _retinueModel.Companions[_companionType].ActiveMission.Value;
            int inventoryAmount, remainingAmount;

            if (_missionItem is CompanionMissionGoodItem goodMissionItem)
            {
                coinCell.gameObject.SetActive(false);
                goodCell.gameObject.SetActive(true);
                coinSubstituteButton.gameObject.SetActive(true);
                deliverTooltip.SetData(notEnoughGoodString.GetLocalizedString());
                var good = goodMissionItem.Good;
                inventoryAmount = _playerModel.Inventory.Get(good);
                remainingAmount = mission.MissionItems[good].RemainingAmount.Value;
                goodCell.Update(good, remainingAmount);
            }
            else
            {
                coinCell.gameObject.SetActive(true);
                goodCell.gameObject.SetActive(false);
                deliverTooltip.SetData(notEnoughCoinString.GetLocalizedString());
                coinSubstituteButton.gameObject.SetActive(false);

                inventoryAmount = Mathf.FloorToInt(_playerModel.Inventory.Funds.Value);
                remainingAmount = mission.CoinCost.RemainingAmount.Value;
                coinCell.SetAmount(remainingAmount);
            }

            var maxDeliverable = Mathf.Min(inventoryAmount, remainingAmount);
            amountSlider.maxValue = remainingAmount;
            amountSlider.value = maxDeliverable;

            RefreshDeliverButton();
            RefreshSubstituteButton();
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }

        private void OnSliderValueChanged(float value)
        {
            var intValue = (int)value;
            sliderValueText.text = intValue.ToString();
            RefreshDeliverButton();
            RefreshSubstituteButton();
        }

        private void OnDeliverClicked()
        {
            var amount = (int)amountSlider.value;
            if (amount <= 0)
                return;

            _deliveryService.Deliver(_missionItem, amount);

            Close();
        }

        private void OnSubstituteClicked()
        {
            var amount = (int)amountSlider.value;
            if (amount <= 0)
                return;

            if (_missionItem is not CompanionMissionGoodItem goodMissionItem)
            {
                Debug.LogError("Substitute payments only work for good mission items. Something went wrong.");
                return;
            }

            _deliveryService.Substitute(goodMissionItem, amount);

            Close();
        }

        private void RefreshDeliverButton()
        {
            var selectedAmount = (int)amountSlider.value;
            var amountInInventory = _missionItem is CompanionMissionGoodItem goodMissionItem
                ? _playerModel.Inventory.Get(goodMissionItem.Good)
                : _playerModel.Inventory.Funds.Value;

            var isInteractable = selectedAmount > 0 && selectedAmount <= amountInInventory;
            deliverButton.interactable = isInteractable;
            deliverTooltip.SetEnabled(!isInteractable && selectedAmount != 0);
        }

        private void RefreshSubstituteButton()
        {
            if (_missionItem is not CompanionMissionGoodItem goodMissionItem)
                return;

            var selectedAmount = (int)amountSlider.value;
            var substituteCost = selectedAmount * goodMissionItem.SubstituteCostSingle;

            var interactable = selectedAmount > 0 && substituteCost <= _playerModel.Inventory.Funds.Value;
            coinSubstituteButton.interactable = interactable;
            coinSubstituteTooltip.SetEnabled(!interactable && selectedAmount != 0);
            coinSubstituteText.text = $"{substituteCost:0.#}";
        }
    }
}