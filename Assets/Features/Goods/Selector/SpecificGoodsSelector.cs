using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;

namespace Features.Goods.Selector
{
    public sealed class SpecificGoodsSelector : IGoodSelector
    {
        private readonly Good[] _good;

        public SpecificGoodsSelector(Good[] good)
        {
            _good = good;
        }

        public bool Matches(Good good)
        {
            return _good.Contains(good);
        }

        public string ToDisplayString()
        {
            var goodResources = ResourceManager.Instance.GoodResources;
            var names = _good.Select(good => goodResources.ResourceData[good].GoodName);
            return $"for {names.JoinWithAnd()}"; // e.g. "for Berries, Logs, Flax and Wheat"
        }
    }
}