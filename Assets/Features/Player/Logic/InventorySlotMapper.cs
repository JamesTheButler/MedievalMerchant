using System.Collections.Generic;
using Common.Types;

namespace Features.Player.Logic
{
    public sealed class InventorySlotMap
    {
        private readonly List<Good?> _slots;

        public IReadOnlyList<Good?> Slots => _slots;

        public InventorySlotMap()
        {
            _slots = new List<Good?>();
            for (var i = 0; i < 16; i++)
            {
                _slots.Add(null);
            }
        }

        public void AddSlot(int index) { }

        public void UpdateSlot(int index, Good? good)
        {
            _slots[index] = good;
        }
    }
}