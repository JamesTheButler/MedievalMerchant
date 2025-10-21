using System;
using System.Collections.Generic;
using Common;
using Common.Config;
using Common.UI;
using Features.Player.Caravan.Config;
using Features.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UI.InventoryUI;
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
        private Image backgroundImage;

        [Header("Header")]
        [SerializeField, Required]
        private TMP_Text moveSpeedText;

        [SerializeField, Required]
        private TMP_Text upkeepText;

        [SerializeField, Required]
        private TMP_Text levelText;

        [SerializeField, Required]
        private SimpleTooltipHandler moveSpeedTooltip, upkeepTooltip;

        [SerializeField, Required]
        private Image moveSpeedUpgradeIcon, upkeepUpgradeIcon;

        [SerializeField, Required]
        private Button upgradeButton;

        [SerializeField, Required]
        private ModifiableTooltipHandler lockedUpgradeTooltip, unlockedUpgradeTooltip;

        [Header("Sprites")]
        [SerializeField, Required]
        private Sprite arrowUp, arrowDown;

        public event Action<InventoryCell> OnCellAdded;
        public event Action<InventoryCell> OnCellClicked;

        private Cart _cart;
        private CaravanConfig _caravanConfig;
        private Colors _colors;

        private int _lastActiveSlotCount;

        public void Bind(Cart cart, Action upgradeAction, Action<InventoryCell> onCellAdded)
        {
            _caravanConfig = ConfigurationManager.Instance.CaravanConfig;
            _colors = ConfigurationManager.Instance.Colors;

            _cart = cart;

            OnCellAdded += onCellAdded;

            ResetSlots();

            _cart.Level.Observe(OnLevelChanged);
            _cart.MoveSpeed.Observe(OnMoveSpeedChanged);
            _cart.Upkeep.Observe(OnUpkeepChanged);
            _cart.SlotCount.Observe(OnSlotCountChanged);

            upgradeButton.onClick.AddListener(() =>
            {
                upgradeAction.Invoke();
                HoverNextLevel();
            });

            unlockButton.onClick.AddListener(upgradeAction.Invoke);
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

        private void SetLocked(bool isLocked)
        {
            unlockButton.gameObject.SetActive(isLocked);
            unlockedParent.gameObject.SetActive(!isLocked);
        }

        private void OnLevelChanged(int level)
        {
            SetLocked(level <= 0);
            upgradeButton.gameObject.SetActive(level < CaravanConfig.MaxLevel);
            UpdateBackgroundImage();
            levelText.text = $"Level {level}";

            if (level >= CaravanConfig.MaxLevel)
            {
                Unhover();
                unlockedUpgradeTooltip.SetEnabled(false);
            }
        }

        private void UpdateBackgroundImage()
        {
            var level = _cart.Level.Value;
            if (level <= 0)
            {
                backgroundImage.sprite = _caravanConfig.DefaultBackgroundImage;
                return;
            }

            var upgradeData = _caravanConfig.GetUpgradeData(level);
            backgroundImage.sprite = upgradeData.BackgroundImage;
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