using System.Collections.Generic;
using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.Types;
using Features.Player.Logic;

namespace Features.Player.Caravan.Logic
{
    public sealed class CaravanSlotService : IService
    {
        private const int CartCount = 4, SlotCount = 4;

        private readonly Dictionary<Good, (int CartIndex, int SlotIndex)> _goodToSlot = new();
        private readonly SortedSet<int> _freeSlots = new();

        private CaravanManager _caravanManager;

        public void Initialize()
        {
            var playerModel = GameplayContext.Instance.Model.Player;
            var inventory = playerModel.Inventory;
            _caravanManager = playerModel.CaravanManager;


            for (var i = 0; i < _caravanManager.Carts.Count; i++)
            {
                var cartIndex = i;
                _caravanManager.Carts[cartIndex].SlotCount.Observe((oldCount, newCount) =>
                {
                    for (var slot = oldCount; slot < newCount; slot++)
                    {
                        _freeSlots.Add(cartIndex * CartCount + slot);
                    }
                });
            }

            inventory.GoodAmountChanged.Observe(OnGoodUpdated);

            foreach (var (good, amount) in inventory.Goods)
            {
                OnGoodAdded(good);
                OnGoodUpdated(good, amount);
            }
        }

        public void CleanUp() { }

        public (int CartIndex, int SlotIndex)? GetSlotForGood(Good good)
        {
            return _goodToSlot.TryGetValue(good, out var slot) ? slot : null;
        }

        private void OnGoodAdded(Good good)
        {
            if (_freeSlots.Count == 0 || _goodToSlot.ContainsKey(good))
                return;

            var firstFreeSlot = _freeSlots.Min;
            _freeSlots.Remove(firstFreeSlot);

            var cartIndex = firstFreeSlot / SlotCount;
            var slotIndex = firstFreeSlot % SlotCount;

            _goodToSlot[good] = (cartIndex, slotIndex);
            _caravanManager.Carts[cartIndex].UpdateSlot(slotIndex, good, 0);
        }

        private void OnGoodUpdated(Good good, int amount)
        {
            if (amount > 0)
            {
                if (!_goodToSlot.TryGetValue(good, out var slot))
                {
                    if (_freeSlots.Count == 0) return;

                    var firstFreeSlot = _freeSlots.Min;
                    _freeSlots.Remove(firstFreeSlot);
                    slot = (firstFreeSlot / SlotCount, firstFreeSlot % SlotCount);
                    _goodToSlot[good] = slot;
                }

                _caravanManager.Carts[slot.CartIndex].UpdateSlot(slot.SlotIndex, good, amount);
            }
            else
            {
                if (!_goodToSlot.Remove(good, out var slot)) return;

                _caravanManager.Carts[slot.CartIndex].ClearSlot(slot.SlotIndex);
                _freeSlots.Add(slot.CartIndex * SlotCount + slot.SlotIndex);
            }
        }
    }
}