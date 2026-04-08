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

        private List<InventoryCell> _emptyCells = new();
        private readonly Dictionary<Good, InventoryCell> _occupiedCells = new();

        private void Awake()
        {
            _emptyCells = GetComponentsInChildren<InventoryCell>().ToList();
            foreach (var cell in _emptyCells)
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

            if (_occupiedCells.TryGetValue(good, out var cell))
            {
                cell.SetAmount(amount);
            }
            else
            {
                var newCell = _emptyCells.FirstOrDefault();
                if (newCell == null)
                {
                    Debug.LogError("No empty cell could be found for camp storage.");
                    return;
                }

                _emptyCells.Remove(newCell);
                _occupiedCells.Add(good, newCell);
                newCell.Update(good, amount);
            }
        }

        private void ClearCell(Good good)
        {
            if (!_occupiedCells.TryGetValue(good, out var cell))
                return;

            cell.Reset();
            _occupiedCells.Remove(good);
            _emptyCells.Add(cell);
        }
    }
}