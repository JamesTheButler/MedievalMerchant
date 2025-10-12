using System;
using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Types;
using Features.Goods.Config;
using UnityEngine;

namespace UI.InventoryUI.TownInventory
{
    public sealed class InventoryTierRow : MonoBehaviour
    {
        public event Action<InventoryCellBase> InventoryCellClicked;

        [SerializeField]
        private Tier tier;

        private GoodsConfig _goodsConfig;

        private readonly Dictionary<Good, InventoryCell> _occupiedCells = new();
        private readonly List<InventoryCell> _inventoryCells = new();

        private void Awake()
        {
            GatherCells();
        }

        private void Start()
        {
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;
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

        public void UpdateGood(Good good, int amount)
        {
            var goodTier = _goodsConfig.ConfigData[good].Tier;
            if (goodTier != tier)
            {
                Debug.LogError($"Tried adding {good} to {nameof(InventoryTierRow)} ({goodTier}) for Tier {tier}.");
                return;
            }

            if (_occupiedCells.TryGetValue(good, out var cell))
            {
                cell.SetAmount(amount);
                if (amount == 0)
                {
                    cell.SetGood(null);
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

            _occupiedCells.Clear();
        }
    }
}