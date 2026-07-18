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
using Features.Map.Pathfinding;
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
        private List<GoodCell> inventoryCells;

        [SerializeField, Required]
        private Button upgradeButton;

        [SerializeField, Required]
        private Button unlockButton;

        [SerializeField, Required]
        private GameObject unlockedParent;

        [SerializeField, Required]
        private GameObject headerGroup;

        [Header("Header")]
        [SerializeField, Required]
        private TMP_Text moveSpeedText;

        [SerializeField, Required]
        private TMP_Text upkeepText;

        [SerializeField]
        private LocalizedString moveSpeedTooltipString, upkeepTooltipString;

        [SerializeField, Required]
        private SimpleTooltipHandler moveSpeedTooltip, upkeepTooltip;

        [SerializeField, Required]
        private CartTooltipHandler cartUnlockTooltip, cartUpgradeTooltip;

        [SerializeField, Required]
        private SimpleTooltipHandler unlockLocationTooltip, upgradeLocationTooltip;

        [SerializeField, Required]
        private Image backgroundImage, cartImage;

        [SerializeField, Required]
        private Image tierIcon, moveSpeedUpgradeIcon, upkeepUpgradeIcon;

        [SerializeField, Required]
        private TMP_Text waggonText;

        [SerializeField]
        private LocalizedString cartString;

        [SerializeField]
        private Color lockedSlotColor = new(0.5f, 0.5f, 0.5f, 0.5f);

        [SerializeField]
        private Color previewSlotColor = new(0.4f, 1f, 0.4f, 0.85f);

        private readonly Bindings _bindings = new();

        private PlayerModel _player;
        private Cart _cart;
        private CaravanResources _caravanResources;
        private CaravanConfig _caravanConfig;
        private bool _isAtCampsite;

        public void Bind(Cart cart, int index, Action upgradeAction, Action unlockAction)
        {
            _player = GameplayContext.Instance.Model.Player;
            _caravanConfig = ConfigurationManager.Configurations.CaravanConfig;
            _caravanResources = ResourceManager.Instance.CaravanResources;
            _cart = cart;

            waggonText.text = cartString.GetLocalizedString(index + 1);

            foreach (var slot in inventoryCells)
            {
                slot.gameObject.SetActive(true);
                slot.SetGood(null);
            }

            _bindings.Track(
                _cart.Level.Observe(OnLevelChanged),
                _cart.MoveSpeed.Observe(OnMoveSpeedChanged),
                _cart.Upkeep.Observe(OnUpkeepChanged),
                _cart.SlotCount.Observe(OnSlotCountChanged),
                _player.Location.MapLocation.Observe(OnPlayerLocationChanged)
            );
            _player.Inventory.Funds.Observe(OnPlayerFundsChanged);

            upgradeButton.onClick.AddListener(() =>
            {
                upgradeAction.Invoke();
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

            for (var i = _cart.SlotCount.Value; i < upgradeData.SlotCount; i++)
            {
                inventoryCells[i].ChangeBackground(previewSlotColor);
            }
        }

        public void Unhover()
        {
            UpdateMoveSpeedText();
            UpdateUpkeepText();

            moveSpeedUpgradeIcon.enabled = false;
            upkeepUpgradeIcon.enabled = false;

            RefreshSlotLocks();
        }

        private void RefreshSlotLocks()
        {
            for (var i = 0; i < inventoryCells.Count; i++)
            {
                var slotColor = i < _cart.SlotCount.Value ? Color.white : lockedSlotColor;
                inventoryCells[i].ChangeBackground(slotColor);
            }
        }

        private void OnLevelChanged(int level)
        {
            SetLocked(level <= 0);
            upgradeButton.gameObject.SetActive(level is > 0 and < CaravanConfig.MaxLevel);
            UpdateCartImage();
            var sprite = _caravanResources.TierIcons.GetValueOrDefault(level, null);
            tierIcon.sprite = sprite;
            tierIcon.color = sprite == null ? Color.clear : Color.white;

            if (level >= CaravanConfig.MaxLevel)
            {
                Unhover();
            }

            RefreshUpgradeButton();
        }

        private void OnPlayerLocationChanged(IMapLocation location)
        {
            _isAtCampsite = _player.Location.IsAtCampsite();
            RefreshUpgradeButton();
            RefreshUnlockButton();
        }

        private void RefreshUpgradeButton()
        {
            var canAffordUpgrade = _cart.UpgradeCost <= _player.Inventory.Funds.Value;
            upgradeButton.interactable = _isAtCampsite && canAffordUpgrade;

            var isUnderMaxLevel = _cart.Level.Value < CaravanConfig.MaxLevel;
            cartUpgradeTooltip.SetEnabled(_isAtCampsite && isUnderMaxLevel);
            upgradeLocationTooltip.SetEnabled(!_isAtCampsite && isUnderMaxLevel);
        }

        private void RefreshUnlockButton()
        {
            unlockButton.interactable = _isAtCampsite;
            cartUnlockTooltip.SetEnabled(_isAtCampsite);
            unlockLocationTooltip.SetEnabled(!_isAtCampsite);
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
            RefreshUpgradeButton();
        }

        private void SetLocked(bool isLocked)
        {
            unlockButton.gameObject.SetActive(isLocked);
            unlockedParent.gameObject.SetActive(!isLocked);
            Fade(isLocked);
            RefreshUnlockButton();
        }

        private void OnSlotCountChanged(int slotCount)
        {
            RefreshSlotLocks();
        }

        private void Fade(bool isFaded)
        {
            backgroundImage.color = isFaded ? Color.white.WithAlpha(0.5f) : Color.white;
        }
    }
}