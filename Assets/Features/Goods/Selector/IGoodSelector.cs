using Common.Types;

namespace Features.Goods.Selector
{
    public interface IGoodSelector
    {
        public static readonly IGoodSelector All = new AllGoodsSelector();

        bool Matches(Good good);

        string ToDisplayString();
    }
}