using Common.Types;

namespace Features.Goods
{
    public interface IGoodSelector
    {
        public bool Matches(Good good);
    }
}