using Common.Types;

namespace Features.Goods.Selector
{
    public interface IGoodSelector
    {
        public bool Matches(Good good);
    }
}