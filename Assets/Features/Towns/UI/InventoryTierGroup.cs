using System;
using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.UI.Elements;
using Common.UI.Tooltips;
using Features.Goods.Config;
using UnityEngine;
using UnityEngine.UI;

namespace Features.Towns.UI
{
    public sealed class InventoryTierGroup : MonoBehaviour
    {
        public event Action<InventoryCellBase> InventoryCellClicked;

        [SerializeField]
        private GameObject lockedGroup;

        [SerializeField]
        private SimpleTooltipHandler lockedTooltip;

        [SerializeField]
        private Image tierIcon;

        [SerializeField]
        private Tier tier;

        private GoodsResources _goodsResources;

        private readonly Dictionary<Good, InventoryCell> _occupiedCells = new();
        private readonly List<InventoryCell> _inventoryCells = new();

        private void Start()
        {
            _goodsResources = ResourceManager.Instance.GoodsResources;
            tierIcon.sprite = ResourceManager.Instance.TierResources.Icons[tier];
            
            GatherCells();
            lockedTooltip.SetData($"Unlocked when town reaches {tier.ToDisplayString()}.");
        }

        public void UpdateGood(Good good, int amount)
        {
            var goodTier = _goodsResources.ResourceData[good].Tier;
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
            foreach (var cell in _occupiedCells)
            {
                cell.Value.Update(null, 0);
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
                inventoryCell.Update(null, 0);
                inventoryCell.Clicked += () => InventoryCellClicked?.Invoke(inventoryCell);
            }
        }
    }
}