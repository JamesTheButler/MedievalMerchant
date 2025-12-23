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
    }
}