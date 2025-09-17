using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using Data.Configuration;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryUI
{
    public sealed class TownInventorySection : MonoBehaviour
    {
        public event Action<InventoryCellBase> InventoryCellClicked;
        public event Action<ProductionCell> UpgradeButtonClicked;

        [SerializeField]
        private Tier tier;

        [SerializeField]
        private Image tierIcon;

        private TierIconConfig _tierIconConfig;
        private GoodsConfig _goodsConfig;

        private readonly Dictionary<Good, InventoryCellBase> _occupiedCells = new();
        private readonly List<ProductionCell> _productionCells = new();
        private readonly List<InventoryCell> _inventoryCells = new();

        private void Awake()
        {
            GatherCells();
        }

        private void Start()
        {
            _tierIconConfig = ConfigurationManager.Instance.TierIconConfig;
            _goodsConfig = ConfigurationManager.Instance.GoodsConfig;

            tierIcon.sprite = _tierIconConfig.Icons[tier];
        }

        private void GatherCells()
        {
            var i = 0;
            foreach (var productionCell in GetComponentsInChildren<ProductionCell>())
            {
                _productionCells.Add(productionCell);
                productionCell.Index = i;
                i++;
                productionCell.UnlockButtonClicked += () => UpgradeButtonClicked?.Invoke(productionCell);
                productionCell.Clicked += () => InventoryCellClicked?.Invoke(productionCell);
            }

            foreach (var inventoryCell in GetComponentsInChildren<InventoryCell>())
            {
                _inventoryCells.Add(inventoryCell);
                inventoryCell.Clicked += () => InventoryCellClicked?.Invoke(inventoryCell);
            }
        }

        public void UpdateProducedGood(Good good, int amount, int index)
        {
            _productionCells[index].Update(good, amount);
        }

        public void UpdateForeignGood(Good good, int amount)
        {
            if (_goodsConfig.ConfigData[good].Tier != tier) return;

            if (_occupiedCells.TryGetValue(good, out var cell))
            {
                cell.SetAmount(amount);
            }
            else
            {
                var freeCell = _inventoryCells.FirstOrDefault(potentiallyFreeCell => !potentiallyFreeCell.HasGood());
                if (freeCell == null)
                {
                    Debug.LogWarning($"There is no free cell for {good}.");
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

            foreach (var productionCell in _productionCells)
            {
                productionCell.Lock();
            }
        }

        public void UnlockProductionCell(int index, Good good)
        {
            if (index >= _productionCells.Count) return;

            var cell = _productionCells[index];
            cell.Unlock();
            cell.Update(good, 0);
            cell.SetEnabled(true);
        }
    }
}