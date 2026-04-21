using System;
using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Observation;
using Common.UI.Elements.Cells;
using Common.UI.Tooltips;
using Common.UI.Utility;
using Common.Utility;
using Features.Goods.Config;
using Features.Player.Caravan.Config;
using Features.Player.Caravan.Logic;
using Features.Player.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Player.Caravan.UI
{
    public sealed class CartStatsUI : MonoBehaviour
    {
        [SerializeField]
        private List<GoodCell> slots;

        [field: SerializeField, Required]
        public Button UpgradeButton { get; private set; }

        [SerializeField, Required]
        private Button unlockButton;

        [SerializeField, Required]
        private GameObject unlockedParent;

        [Header("Header")]
        [SerializeField, Required]
        private TMP_Text moveSpeedText, upkeepText;

        [SerializeField]
        private LocalizedString moveSpeedTooltipString, upkeepTooltipString;

        [SerializeField, Required]
        private SimpleTooltipHandler moveSpeedTooltip, upkeepTooltip;

        [SerializeField, Required]
        private CartTooltipHandler cartUnlockTooltip, cartUpgradeTooltip;

        [SerializeField, Required]
        private Image backgroundImage, cartImage;

        [SerializeField, Required]
        private Image tierIcon, moveSpeedUpgradeIcon, upkeepUpgradeIcon, faderImage;

        private readonly Bindings _bindings = new();

        private PlayerModel _player;
        private Cart _cart;
        private CaravanResources _caravanResources;
        private CaravanConfig _caravanConfig;

        private int _lastActiveSlotCount;

        public void Bind(Cart cart, int index, Action upgradeAction, Action unlockAction)
        {
            _player = GameplayContext.Instance.Model.Player;
            _caravanConfig = ConfigurationManager.Configurations.CaravanConfig;
            _caravanResources = ResourceManager.Instance.CaravanResources;

            _bindings.Track(
                _cart.Level.Observe(OnLevelChanged),
                _cart.MoveSpeed.Observe(OnMoveSpeedChanged),
                _cart.Upkeep.Observe(OnUpkeepChanged),
                _cart.SlotCount.Observe(OnSlotCountChanged)
            );
            _player.Inventory.Funds.Observe(OnPlayerFundsChanged);

            UpgradeButton.onClick.AddListener(() =>
            {
                upgradeAction.Invoke();
                cartUpgradeTooltip.SetData(_cart);
                HoverNextLevel();
            });

            unlockButton.onClick.AddListener(unlockAction.Invoke);
            cartUpgradeTooltip.SetData(_cart);
            cartUnlockTooltip.SetData(_cart);
            Unhover();
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

        private void OnLevelChanged(int level)
        {
            SetLocked(level <= 0);
            UpgradeButton.gameObject.SetActive(level < CaravanConfig.MaxLevel);
            UpdateCartImage();
            var sprite = _caravanResources.TierIcons.GetValueOrDefault(level, null);
            tierIcon.sprite = sprite;
            tierIcon.color = sprite == null ? Color.clear : Color.white;

            if (level >= CaravanConfig.MaxLevel)
            {
                Unhover();
                cartUpgradeTooltip.SetEnabled(false);
            }
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
            var style = isBigger == isBiggerBetter ? Style.Good : Style.Bad;

            textField.text = newValue.ToString("N0").WithStyle(style);
        }

        private void UpdateMoveSpeedText()
        {
            var moveSpeed = _cart.MoveSpeed.Value.ToString("N0");
            moveSpeedText.text = moveSpeed;
            moveSpeedTooltip.SetData(moveSpeedTooltipString.GetLocalizedString(moveSpeed));
        }

        private void UpdateUpkeepText()
        {
            var upkeep = _cart.Upkeep.Value.ToString("N0");
            upkeepText.text = upkeep;
            upkeepTooltip.SetData(upkeepTooltipString.GetLocalizedString(upkeep));
        }

        private void OnMoveSpeedChanged(float moveSpeed)
        {
            UpdateMoveSpeedText();
        }

        private void OnUpkeepChanged(float upkeep)
        {
            UpdateUpkeepText();
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

        private void OnPlayerFundsChanged(float funds)
        {
            UpgradeButton.interactable = _cart.UpgradeCost <= funds;
        }

        private void SetLocked(bool isLocked)
        {
            unlockButton.gameObject.SetActive(isLocked);
            unlockedParent.gameObject.SetActive(!isLocked);
            Fade(isLocked);
        }

        private void OnSlotCountChanged(int slotCount)
        {
            if (slotCount < _lastActiveSlotCount)
            {
                Debug.LogError("Slot count reduction is not supported!.");
            }

            for (var slotIndex = _lastActiveSlotCount; slotIndex < slotCount; slotIndex++)
            {
                slots[slotIndex].gameObject.SetActive(true);
                slots[slotIndex].SetGood(null);
            }

            _lastActiveSlotCount = slotCount;
        }

        private void Fade(bool isFaded)
        {
            faderImage.enabled = isFaded;
            backgroundImage.color = isFaded ? Color.white.WithAlpha(0.5f) : Color.white;
        }
    }
}