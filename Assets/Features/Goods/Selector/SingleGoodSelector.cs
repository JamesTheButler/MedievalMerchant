using Common.Infrastructure;
using Common.Types;

namespace Features.Goods.Selector
{
    public sealed class SingleGoodSelector : IGoodSelector
    {
        private readonly Good _good;

        public SingleGoodSelector(Good good)
        {
            _good = good;
        }

        public bool Matches(Good good)
        {
            return _good == good;
        }
        
        public string ToDisplayString()
        {
            var data = ResourceManager.Instance.GoodResources.ResourceData[_good];
            return $"for {data.GoodName}"; // e.g. "for Berries"
        }
    }
}