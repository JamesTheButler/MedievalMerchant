using Common.Types;

namespace Features.Player.Logic
{
    public static class InventorySlotMapperExtension
    {
        public static (int CartIndex, int SlotIndex) GetOrAddSlotIndexC(this InventorySlotMapper self, Good good)
        {
            var index = self.GetOrAddSlotIndex(good);
            var cartIndex = index % 4;
            var slotIndex = index - cartIndex * 4;
            return (cartIndex, slotIndex);
        }
    }
}