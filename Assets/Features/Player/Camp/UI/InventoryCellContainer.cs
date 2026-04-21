using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure.Observation;
using Common.Types;
using Common.UI.Elements.Cells;
using UnityEngine;

namespace Features.Player.Camp.UI
{
    public sealed class InventoryCellContainer : MonoBehaviour
    {
        public ObservableEvent<InventoryCell> OnCellClicked { get; } = new();

        private List<InventoryCell> _inventoryCells = new();

        private void Awake()
        {
            _inventoryCells = GetComponentsInChildren<InventoryCell>().ToList();
            foreach (var cell in _inventoryCells)
            {
                cell.Reset();
                cell.Clicked += () => OnCellClicked.Invoke(cell);
            }
        }

        public void UpdateGood(Good good, int amount)
        {
            if (amount <= 0)
            {
                ClearCell(good);
                return;
            }

            var occupiedCell = TryGetCell(good);
            if (occupiedCell != null)
            {
                occupiedCell.SetAmount(amount);
            }
            else
            {
                var newCell = _inventoryCells.First(cell => !cell.HasGood());
                if (newCell == null)
                {
                    Debug.LogError("No empty cell could be found for camp storage.");
                    return;
                }

                newCell.Update(good, amount);
            }
        }

        private void ClearCell(Good good)
        {
            var occupiedCell = TryGetCell(good);
            if (occupiedCell != null)
            {
                occupiedCell.Reset();
            }
        }

        private InventoryCell TryGetCell(Good good)
        {
            return _inventoryCells.FirstOrDefault(cell => cell.Good == good);
        }
    }
}