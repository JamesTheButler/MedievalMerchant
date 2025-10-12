using Common.Types;
using JetBrains.Annotations;

namespace UI.InventoryUI
{
    [PublicAPI]
    public static class InventoryCellExtensions
    {
        public static void Update(this InventoryCellBase cell, Good? good, int amount)
        {
            cell.SetGood(good);
            cell.SetAmount(amount);
        }

        public static void Reset(this InventoryCellBase cell)
        {
            cell.Update(null, 0);
        }

        public static bool HasGood(this InventoryCellBase cell)
        {
            return cell.Good != null;
        }
    }
}