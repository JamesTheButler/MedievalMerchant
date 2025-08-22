using System.Collections.Generic;
using Data;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI
{
    public class InventoryUI : MonoBehaviour
    {
        [SerializeField]
        private TMP_Text fundsText;

        [SerializeField]
        private InventorySection inventorySectionT1;

        [SerializeField]
        private InventorySection inventorySectionT2;

        [SerializeField]
        private InventorySection inventorySectionT3;

        private readonly Dictionary<Good, InventoryCell> _inventoryCells = new();

        private Inventory _boundInventory;

        public void Bind(Inventory inventory)
        {
            UnBind();

            inventory.GoodUpdated += OnGoodUpdated;
            inventory.FundsUpdated += OnFundsUpdated;

            // initialize
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

            foreach (var (_, cell) in _inventoryCells)
            {
                cell.SetAmount(0);
                cell.SetIsProduced(false);
            }

            fundsText.text = "0";
        }

        public void Show()
        {
            gameObject.SetActive(true);
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        public void Hide()
        {
            gameObject.SetActive(false);
        }

        private void Start()
        {
            CollectInventoryCells(inventorySectionT1);
            CollectInventoryCells(inventorySectionT2);
            CollectInventoryCells(inventorySectionT3);

            Hide();
            LayoutRebuilder.ForceRebuildLayoutImmediate(transform as RectTransform);
        }

        private void OnFundsUpdated(int funds)
        {
            fundsText.text = funds.ToString();
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            if (!_inventoryCells.TryGetValue(good, out var cell))
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
                _inventoryCells.Add(good, cell);
            }
        }
    }
}