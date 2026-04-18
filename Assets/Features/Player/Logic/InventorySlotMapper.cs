using System.Collections.Generic;
using Common.Types;
using Common.Utility;
using UnityEngine;

namespace Features.Player.Logic
{
    public sealed class InventorySlotMapper
    {
        private readonly SortedList<int, int> _freeSlotIndices = new();
        private readonly Dictionary<Good, int> _occupiedCells = new();

        public void AddSlot(int index)
        {
            _freeSlotIndices.Add(index, index);
        }

        public int GetOrAddSlotIndex(Good good)
        {
            return _occupiedCells.TryGetValue(good, out var index)
                ? index
                : AddGood(good);
        }

        public void RemoveGood(Good good)
        {
            if (!_occupiedCells.TryGetValue(good, out var index))
                Debug.LogError("Good is not tracked.");

            _occupiedCells.Remove(good);
            _freeSlotIndices.Remove(index);
        }

        private int GetNextFreeSlot()
        {
            var index = _freeSlotIndices[0];
            _freeSlotIndices.RemoveAt(0);
            return index;
        }

        private int AddGood(Good good)
        {
            if (_occupiedCells.ContainsKey(good))
            {
                Debug.LogError("Good is already tracked.");
                return -1;
            }

            if (_freeSlotIndices.IsEmpty())
            {
                Debug.LogError("No more space.");
                return -1;
            }

            var index = GetNextFreeSlot();
            _occupiedCells.Add(good, index);
            return index;
        }
    }
}