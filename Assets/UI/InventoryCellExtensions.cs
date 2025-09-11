using Data;
using JetBrains.Annotations;

namespace UI
{
    [PublicAPI]
    public static class InventoryCellExtensions
    {
        public static void Update(this InventoryCell cell, Good? good, int amount)
        {
            cell.SetGood(good);
            cell.SetAmount(amount);
        }

        public static void Reset(this InventoryCell cell)
        {
            cell.Update(null, 0);
        }

        public static bool HasGood(this InventoryCell cell)
        {
            return cell.Good != null;
        }
    }
}