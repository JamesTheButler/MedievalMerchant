using System;
using System.Collections.Generic;
using Common.Config;
using Common.Infrastructure;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Goods.Config;
using Features.Player.Caravan.Config;
using Features.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Player.Caravan.UI
{
    public sealed class CartUI : MonoBehaviour
    {
        [SerializeField]
        private List<InventoryCell> inventoryCells;

        [SerializeField, Required]
        private GameObject unlockedParent;

        [SerializeField, Required]
        private Button unlockButton;

        [SerializeField, Required]
        private Image backgroundImage, cartImage;

        [Header("Header")]
        [SerializeField, Required]
        private TMP_Text moveSpeedText, upkeepText;

        [SerializeField, Required]
        private SimpleTooltipHandler moveSpeedTooltip, upkeepTooltip;

        [SerializeField, Required]
        private Image tierIcon, moveSpeedUpgradeIcon, upkeepUpgradeIcon, faderImage;

        [SerializeField, Required]
        private Button upgradeButton;

        [SerializeField, Required]
        private ModifiableTooltipHandler lockedUpgradeTooltip, unlockedUpgradeTooltip;

        [Header("Sprites")]
        [SerializeField, Required]
        private Sprite arrowUp, arrowDown;

        public event Action<InventoryCell> OnCellAdded, OnCellClicked;

        private PlayerModel _player;
        private Cart _cart;
        private CaravanConfig _caravanConfig;
        private CaravanResources _caravanResources;
        private Colors _colors;

        private int _lastActiveSlotCount;

        public void Bind(Cart cart, Action upgradeAction, Action unlockAction, Action<InventoryCell> onCellAdded)
        {
            _player = GameplayContext.Instance.Model.Player;
            _caravanConfig = ConfigurationManager.Configurations.CaravanConfig;
            _caravanResources = ResourceManager.Instance.CaravanResources;
            _colors = ResourceManager.Instance.Colors;

            _cart = cart;

            OnCellAdded += onCellAdded;

            ResetSlots();

            _cart.Level.Observe(OnLevelChanged);
            _cart.MoveSpeed.Observe(OnMoveSpeedChanged);
            _cart.Upkeep.Observe(OnUpkeepChanged);
            _cart.SlotCount.Observe(OnSlotCountChanged);

            _player.Inventory.Funds.Observe(OnPlayerFundsChanged);

            upgradeButton.onClick.AddListener(() =>
            {
                upgradeAction.Invoke();
                HoverNextLevel();
            });

            unlockButton.onClick.AddListener(unlockAction.Invoke);
            lockedUpgradeTooltip.SetData(_cart.UpgradeCost);
            unlockedUpgradeTooltip.SetData(_cart.UpgradeCost);
            Unhover();
        }

        public void Unbind()
        {
            if (_cart == null)
                return;

            _cart.Level.StopObserving(OnLevelChanged);
            _cart.MoveSpeed.StopObserving(OnMoveSpeedChanged);
            _cart.Upkeep.StopObserving(OnUpkeepChanged);
            _cart.SlotCount.StopObserving(OnSlotCountChanged);
            _cart = null;
        }

        public void HoverNextLevel()
        {
            var level = _cart.Level + 1;
            if (level > CaravanConfig.MaxLevel)
                return;

            var upgradeData = _caravanConfig.GetUpgradeData(level);

            HoverTextfield(moveSpeedText, _cart.MoveSpeed, upgradeData.MoveSpeed, true);
            HoverTextfield(upkeepText, _cart.Upkeep, upgradeData.Upkeep, false);

            moveSpeedUpgradeIcon.enabled = true;
            upkeepUpgradeIcon.enabled = true;
        }

        public void Unhover()
        {
            UpdateMoveSpeedText();
            UpdateUpkeepText();

            moveSpeedUpgradeIcon.enabled = false;
            upkeepUpgradeIcon.enabled = false;
        }

        private void OnPlayerFundsChanged(float funds)
        {
            upgradeButton.interactable = _cart.UpgradeCost <= funds;
        }

        private void SetLocked(bool isLocked)
        {
            unlockButton.gameObject.SetActive(isLocked);
            unlockedParent.gameObject.SetActive(!isLocked);
            Fade(isLocked);
        }

        private void Fade(bool isFaded)
        {
            faderImage.enabled = isFaded;
            backgroundImage.color = isFaded ? Color.white.WithAlpha(0.5f) : Color.white;
        }

        private void OnLevelChanged(int level)
        {
            SetLocked(level <= 0);
            upgradeButton.gameObject.SetActive(level < CaravanConfig.MaxLevel);
            UpdateCartImage();
            var sprite = _caravanResources.TierIcons.GetValueOrDefault(level, null);
            tierIcon.sprite = sprite;
            // hide make icon transparent if not shown
            tierIcon.color = sprite == null ? Color.clear : Color.white;

            if (level >= CaravanConfig.MaxLevel)
            {
                Unhover();
                unlockedUpgradeTooltip.SetEnabled(false);
            }
        }

        private void UpdateCartImage()
        {
            var level = _cart.Level.Value;
            if (level <= 0)
            {
                cartImage.sprite = null;
                cartImage.enabled = false;
                return;
            }

            var backgroundSprite = _caravanResources.BackgroundImages[level];
            cartImage.sprite = backgroundSprite;
            cartImage.enabled = true;
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            UpdateMoveSpeedText();
        }

        private void OnUpkeepChanged(float upkeep)
        {
            UpdateUpkeepText();
        }

        private void ResetSlots()
        {
            foreach (var slot in inventoryCells)
            {
                slot.gameObject.SetActive(false);
                slot.Reset();
            }

            _lastActiveSlotCount = 0;
        }

        private void OnSlotCountChanged(int slotCount)
        {
            if (slotCount < _lastActiveSlotCount)
            {
                Debug.LogError("Slot count reduction is not supported!.");
            }

            for (var slotIndex = _lastActiveSlotCount; slotIndex < slotCount; slotIndex++)
            {
                var cell = inventoryCells[slotIndex];
                cell.gameObject.SetActive(true);
                OnCellAdded?.Invoke(cell);
                cell.Clicked += () => OnCellClicked?.Invoke(cell);
            }

            _lastActiveSlotCount = slotCount;
        }

        private void UpdateMoveSpeedText()
        {
            var moveSpeed = _cart.MoveSpeed.Value.ToString("N0");
            moveSpeedText.text = moveSpeed;
            moveSpeedText.color = _colors.FontDark;
            moveSpeedTooltip.SetData($"Movement Speed: {moveSpeed}");
        }

        private void UpdateUpkeepText()
        {
            var upkeep = _cart.Upkeep.Value.ToString("N0");
            upkeepText.text = upkeep;
            upkeepText.color = _colors.FontDark;
            upkeepTooltip.SetData($"Upkeep: {upkeep}");
        }

        private void HoverTextfield(
            TMP_Text textField,
            float oldValue,
            float newValue,
            bool isBiggerBetter)
        {
            if (oldValue.IsApproximately(newValue))
                return;

            var isBigger = newValue > oldValue;
            var color = isBigger == isBiggerBetter ? _colors.Good : _colors.Bad;

            textField.text = newValue.ToString("N0");
            textField.color = color;
        }
    }
}