using Common.Types;

namespace Features.Goods
{
    public sealed class SpecificGoodSelector : IGoodSelector
    {
        private readonly Good _good;

        public SpecificGoodSelector(Good good)
        {
            _good = good;
        }
        public bool Matches(Good good)
        {
            return _good == good;
        }
    }
}