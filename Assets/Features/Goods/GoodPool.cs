using System.Collections.Generic;
using Common.Types;
using Common.Utility;

namespace Features.Goods
{
    public sealed class GoodPool
    {
        private readonly IEnumerable<Good> _availableGoods = EnumExtensions.Enumerate<Good>();

        public IEnumerable<Good> GetAvailableGoods()
        {
            return _availableGoods;
        }
    }
}