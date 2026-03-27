using Common.Infrastructure.Gameplay;
using Common.Types;
using Common.UI.Elements.Cells;
using Common.UI.Elements.Panels;
using Common.UI.InventoryUI;
using Features.Player.Logic;
using Features.Player.Retinue;
using Features.Player.Retinue.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.Camp.UI
{
    public sealed class CompanionDeliveryPanel : DynamicPanel
    {
        [SerializeField, Required]
        private Slider amountSlider;

        [SerializeField, Required]
        private TMP_Text sliderValueText;

        [SerializeField, Required]
        private InventoryCell goodCell;

        [SerializeField, Required]
        private CoinCell coinCell;

        [SerializeField, Required]
        private Button deliverButton;

        private PlayerModel _playerModel;
        private RetinueModel _retinueModel;
        private CompanionDeliveryService _deliveryService;

        private CompanionType _companionType;
        private Good? _good;
        private bool _isCoinDelivery;

        protected override void OnInitialize()
        {
            _playerModel = GameplayContext.Instance.Model.Player;
            _retinueModel = _playerModel.RetinueModel;
            _deliveryService = GameplayContext.Instance.Services.CompanionDeliveryService;

            deliverButton.onClick.AddListener(OnDeliverClicked);
            amountSlider.onValueChanged.AddListener(OnSliderValueChanged);
        }

        public void SetUpForGood(CompanionType companionType, Good good)
        {
            _companionType = companionType;
            _good = good;
            _isCoinDelivery = false;
        }

        public void SetUpForCoin(CompanionType companionType)
        {
            _companionType = companionType;
            _good = null;
            _isCoinDelivery = true;
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);

            coinCell.gameObject.SetActive(_isCoinDelivery);
            goodCell.gameObject.SetActive(!_isCoinDelivery);

            var mission = _retinueModel.Companions[_companionType].ActiveMission.Value;
            int inventoryAmount, remainingAmount;

            if (_isCoinDelivery)
            {
                inventoryAmount = Mathf.FloorToInt(_playerModel.Inventory.Funds.Value);
                remainingAmount = mission.CoinCost.RemainingAmount.Value;
                coinCell.SetAmount(remainingAmount);
            }
            else
            {
                var good = _good!.Value;
                inventoryAmount = _playerModel.Inventory.Get(good);
                remainingAmount = mission.MissionItems[good].RemainingAmount.Value;
                goodCell.Update(good, remainingAmount);
            }

            var maxDeliverable = Mathf.Min(inventoryAmount, remainingAmount);
            amountSlider.maxValue = maxDeliverable;
            amountSlider.value = maxDeliverable;
            //OnSliderValueChanged(maxDeliverable);

            RefreshDeliverButton();
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
        }

        private void OnDeliverClicked()
        {
            var amount = (int)amountSlider.value;
            if (amount <= 0)
                return;

            if (_isCoinDelivery)
            {
                _deliveryService.DeliverCoin(_companionType, amount);
            }
            else
            {
                _deliveryService.DeliverGood(_companionType, _good!.Value, amount);
            }

            Close();
        }

        private void RefreshDeliverButton()
        {
            deliverButton.interactable = (int)amountSlider.value > 0;
        }
    }
}