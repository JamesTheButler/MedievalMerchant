using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent<InventoryCell> inventoryCellClicked;

        [SerializeField]
        private TMP_Text fundsText;

        [SerializeField]
        private InventorySection inventorySectionT1;

        [SerializeField]
        private InventorySection inventorySectionT2;

        [SerializeField]
        private InventorySection inventorySectionT3;

        protected readonly Dictionary<Good, InventoryCell> InventoryCells = new();

        private Inventory _boundInventory;

        private bool _isInitialized;

        public void Bind(Inventory inventory)
        {
            if (!_isInitialized)
            {
                CollectInventoryCells(inventorySectionT1);
                CollectInventoryCells(inventorySectionT2);
                CollectInventoryCells(inventorySectionT3);
                _isInitialized = true;
            }

            UnBind();

            inventory.GoodUpdated += OnGoodUpdated;
            inventory.FundsUpdated += OnFundsUpdated;

            foreach (var (good, amount) in inventory.Goods)
            {
                OnGoodUpdated(good, amount);
            }

            OnFundsUpdated(inventory.Funds);

            _boundInventory = inventory;
        }

        public void UnBind()
        {
            if (_boundInventory != null)
            {
                _boundInventory.GoodUpdated -= OnGoodUpdated;
                _boundInventory.FundsUpdated -= OnFundsUpdated;

                _boundInventory = null;
            }

            foreach (var (_, cell) in InventoryCells)
            {
                cell.SetAmount(0);
                cell.SetIsProduced(false);
            }

            fundsText.text = "0";
        }

        public void Show()
        {
            gameObject.SetActive(true);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void OnFundsUpdated(int funds)
        {
            fundsText.text = funds.ToString();
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            if (!InventoryCells.TryGetValue(good, out var cell))
            {
                Debug.LogWarning($"Could not find inventory cell for good '{good}'");
                return;
            }

            cell.SetAmount(amount);
        }

        private void CollectInventoryCells(InventorySection inventorySection)
        {
            foreach (var (good, cell) in inventorySection.Cells)
            {
                InventoryCells.Add(good, cell);
                cell.Clicked += () => inventoryCellClicked?.Invoke(cell);
            }
        }
    }
}