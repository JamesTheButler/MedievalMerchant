using Common.Types;

namespace Features.Goods
{
    public sealed class AllGoodsSelector : IGoodSelector
    {
        public bool Matches(Good good)
        {
            return true;
        }
    }
}