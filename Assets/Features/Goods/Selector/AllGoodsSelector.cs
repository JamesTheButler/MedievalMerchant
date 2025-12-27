using Common.Types;

namespace Features.Goods.Selector
{
    public sealed class AllGoodsSelector : IGoodSelector
    {
        public bool Matches(Good good)
        {
            return true;
        }

        public string ToDisplayString()
        {
            return "for all goods";
        }
    }
}