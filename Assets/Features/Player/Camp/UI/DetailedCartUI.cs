using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Common.UI.Elements.Cells;
using Features.Goods.Config;
using Features.Inventory;
using Features.Player.Caravan.Logic;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Player.Camp.UI
{
    public sealed class DetailedCartUI : MonoBehaviour
    {
        [SerializeField]
        private List<InventoryCell> inventoryCells;

        [SerializeField, Required]
        private Image cartImage;

        [SerializeField, Required]
        private TMP_Text waggonText, moveSpeedText, upkeepText;

        [SerializeField, Required]
        private Button upgradeButton;

        [SerializeField]
        private LocalizedString cartString;

        [SerializeField, Required]
        private Image tierIcon;

        private Cart _cart;
        private CaravanResources _caravanResources;
        private readonly Bindings _cartBindings = new(), _slotBindings = new();

        private int _lastActiveSlotCount;

        public void Bind(Cart cart, int index)
        {
            _caravanResources = ResourceManager.Instance.CaravanResources;

            _cart = cart;
            waggonText.text = cartString.GetLocalizedString(index + 1);

            ResetSlots();

            _cartBindings.Track(
                _cart.Level.Observe(OnLevelChanged),
                _cart.SlotCount.Observe(OnSlotCountChanged),
                _cart.Upkeep.Observe(OnUpkeepChanged),
                _cart.MoveSpeed.Observe(OnMoveSpeedChanged)
            );

            for (var slotIndex = 0; slotIndex < cart.Slots.Length; slotIndex++)
            {
                var cellIndex = slotIndex;
                var binding = cart.Slots[slotIndex].Observe(entry => OnSlotChanged(cellIndex, entry));
                _slotBindings.Track(binding);
            }
        }

        private void OnUpkeepChanged(float upkeep)
        {
            upkeepText.text = upkeep.ToString("0.##");
        }

        private void OnMoveSpeedChanged(float movespeed)
        {
            moveSpeedText.text = movespeed.ToString("0.#");
        }

        public void Unbind()
        {
            if (_cart == null)
                return;

            _slotBindings.Unbind();

            _cart.Level.StopObserving(OnLevelChanged);
            _cart.SlotCount.StopObserving(OnSlotCountChanged);
            _cart = null;
        }

        public InventoryCell GetInventoryCell(int slotIndex)
        {
            return inventoryCells[slotIndex];
        }

        private void OnSlotChanged(int cellIndex, InventoryEntry entry)
        {
            var cell = inventoryCells[cellIndex];
            if (entry != null)
            {
                cell.Update(entry.Good, entry.Amount);
            }
            else
            {
                cell.Reset();
            }
        }

        private void OnLevelChanged(int level)
        {
            SetLocked(level <= 0);
            UpdateCartImage();
            var sprite = _caravanResources.TierIcons.GetValueOrDefault(level, null);
            tierIcon.sprite = sprite;
            tierIcon.color = sprite == null ? Color.clear : Color.white;
        }

        private void SetLocked(bool isLocked)
        {
            gameObject.SetActive(!isLocked);
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
                return;
            }

            for (var slotIndex = _lastActiveSlotCount; slotIndex < slotCount; slotIndex++)
            {
                inventoryCells[slotIndex].gameObject.SetActive(true);
                inventoryCells[slotIndex].Reset();
            }

            _lastActiveSlotCount = slotCount;
        }
    }
}