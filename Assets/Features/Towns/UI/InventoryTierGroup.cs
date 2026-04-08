using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements.Cells;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Goods.Config;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.UI;

namespace Features.Towns.UI
{
    public sealed class InventoryTierGroup : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject lockedGroup;

        [SerializeField, Required]
        private SimpleTooltipHandler lockedTooltip;

        [SerializeField, Required]
        private Image tierIcon;

        [SerializeField]
        private Tier tier;

        [SerializeField]
        private LocalizedString lockedString;

        private GoodResources _goodResources;

        private readonly Dictionary<Good, InventoryCell> _occupiedCells = new();
        private readonly List<InventoryCell> _inventoryCells = new();

        private void Start()
        {
            _goodResources = ResourceManager.Instance.GoodResources;
            tierIcon.sprite = ResourceManager.Instance.TierResources.Icons[tier];

            GatherCells();
            lockedTooltip.SetData(lockedString.GetLocalizedString(new { TierRoman = tier.ToRomanNumeral() }));
        }

        public void UpdateGood(Good good, int amount)
        {
            var goodTier = _goodResources.ResourceData[good].Tier;
            if (goodTier != tier)
            {
                Debug.LogError($"Tried adding {good} to {nameof(InventoryTierGroup)} ({goodTier}) for Tier {tier}.");
                return;
            }

            if (_occupiedCells.TryGetValue(good, out var cell))
            {
                cell.SetAmount(amount);
                if (amount == 0)
                {
                    cell.Reset();
                    _occupiedCells.Remove(good);
                }
            }
            else
            {
                if (amount == 0)
                    return;

                var freeCell = _inventoryCells.FirstOrDefault(potentiallyFreeCell => !potentiallyFreeCell.HasGood());
                if (freeCell == null)
                {
                    Debug.LogError($"There is no free cell for {good}.");
                    return;
                }

                _occupiedCells.Add(good, freeCell);
                freeCell.Update(good, amount);
            }
        }

        public void Reset()
        {
            foreach (var cell in _inventoryCells)
            {
                cell.Reset();
            }

            SetLocked(true);
            _occupiedCells.Clear();
        }

        public void SetLocked(bool isLocked)
        {
            lockedGroup.SetActive(isLocked);
        }

        private void GatherCells()
        {
            foreach (var inventoryCell in GetComponentsInChildren<InventoryCell>())
            {
                _inventoryCells.Add(inventoryCell);
                inventoryCell.Reset();
            }
        }
    }
}