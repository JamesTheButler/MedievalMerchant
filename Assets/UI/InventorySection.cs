using System;
using System.Collections.Generic;
using System.Linq;
using Data;
using NaughtyAttributes;
using UnityEngine;

namespace UI
{
    /// <summary>
    /// Responsible for a section of cells in an inventory. Controls the display of the given goods.
    /// </summary>
    public sealed class InventorySection : MonoBehaviour
    {
        [SerializeField, Required]
        private Transform parent;

        [SerializeField, Required]
        private GameObject cellPrefab;

        [SerializeField]
        private int minCellCount = 6;

        [SerializeField]
        private bool resizeDynamically = true;

        [SerializeField, ShowIf(nameof(resizeDynamically))]
        private int newCellCount = 3;

        public event Action<InventoryCell> CellClicked;

        private readonly List<InventoryCell> _allCells = new();
        private readonly Dictionary<Good, InventoryCell> _cellsPerGoods = new();

        public void Initialize()
        {
            CleanUp();

            AddNewCells(minCellCount);
        }

        public void CleanUp()
        {
            foreach (var cell in _allCells)
            {
                Destroy(cell.gameObject);
            }

            _allCells.Clear();
            _cellsPerGoods.Clear();
        }

        public void UpdateGood(Good good, int amount, bool isProduced)
        {
            // free up the cell if
            if (amount <= 0)
            {
                _cellsPerGoods.Remove(good, out var cell);
                cell?.Reset();
                return;
            }

            // Find next free cell or create new ones if needed.
            if (!_cellsPerGoods.ContainsKey(good))
            {
                if (resizeDynamically && _allCells.All(potentiallyFreeCell => potentiallyFreeCell.HasGood()))
                {
                    AddNewCells(newCellCount);
                }

                var freeCell = _allCells.First(potentiallyFreeCell => !potentiallyFreeCell.HasGood());
                freeCell.SetGood(good);
                _cellsPerGoods.Add(good, freeCell);
            }

            // update good info
            _cellsPerGoods[good].SetAmount(amount);
            _cellsPerGoods[good].SetIsProduced(isProduced);
        }

        private void AddNewCells(int count)
        {
            for (var i = 0; i < count; i++)
            {
                var cellObject = Instantiate(cellPrefab, parent);
                var cell = cellObject.GetComponent<InventoryCell>();
                cell.Reset();
                cell.Clicked += () => InvokeClickEvent(cell);
                _allCells.Add(cell);
            }
        }

        private void InvokeClickEvent(InventoryCell cell)
        {
            if (cell.Good == null) return;

            CellClicked?.Invoke(cell);
        }
    }
}