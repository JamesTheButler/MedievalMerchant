using System;
using System.Collections.Generic;
using Common;
using Common.Config;
using Common.Types;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.InventoryUI.TownInventory
{
    public sealed class ProductionTierRow : MonoBehaviour
    {
        public event Action<ProductionCell> UpgradeButtonClicked;
        public event Action<ProductionCell> ProductionCellClicked;

        private readonly List<ProductionCell> _productionCells = new();

        private TierIconConfig _tierIconConfig;

        [SerializeField]
        private Tier tier;

        [SerializeField, Required]
        private Image tierIcon;

        private void Awake()
        {
            GatherCells();
        }

        private void Start()
        {
            _tierIconConfig = ConfigurationManager.Instance.TierIconConfig;

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
                productionCell.EnableUpgradeButton(false);
                productionCell.Update(null, 0);
                productionCell.UnlockButtonClicked += () => UpgradeButtonClicked?.Invoke(productionCell);
                productionCell.Clicked += () => ProductionCellClicked?.Invoke(productionCell);
            }
        }

        public void UpdateProducedGood(Good good, int amount, int index)
        {
            _productionCells[index].Update(good, amount);
        }


        public void Reset()
        {
            foreach (var productionCell in _productionCells)
            {
                productionCell.EnableUpgradeButton(false);
                productionCell.Lock();
                productionCell.Update(null, 0);
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

        public void EnableProductionCellUpgradeButton(int index, bool isEnabled)
        {
            if (index >= _productionCells.Count) return;

            _productionCells[index].EnableUpgradeButton(isEnabled);
        }

        public void EnableProductionCellUpgradeButtons(bool isEnabled)
        {
            foreach (var cell in _productionCells)
            {
                cell.EnableUpgradeButton(isEnabled);
            }
        }
    }
}