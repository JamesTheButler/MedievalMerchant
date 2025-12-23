using System.Linq;
using Common.Infrastructure;
using Common.Types;
using Common.Utility;

namespace Features.Goods.Selector
{
    public sealed class SpecificGoodsSelector : IGoodSelector
    {
        private readonly Good[] _good;

        public string Description { get; }

        public SpecificGoodsSelector(Good[] good)
        {
            _good = good;
            Description = GetDescription();
        }

        public bool Matches(Good good)
        {
            return _good.Contains(good);
        }

        private string GetDescription()
        {
            var goodResources = ResourceManager.Instance.GoodsResources;
            var names = _good.Select(good => goodResources.ConfigData[good].GoodName);
            return names.JoinWithAnd();
        }
    }
}