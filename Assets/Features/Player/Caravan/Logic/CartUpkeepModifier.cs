using Common.Infrastructure;
using Common.Infrastructure.Modifiable;

namespace Features.Player.Caravan.Logic
{
    public sealed class CartUpkeepModifier : FlatModifier
    {
        private readonly int _index;

        public CartUpkeepModifier(int index, float value, int cartLevel) : base(value, GetDescription(index, cartLevel))
        {
            _index = index;
        }

        public void Update(float value, int cartLevel)
        {
            Value.Value = value;
            Description.Value = GetDescription(_index, cartLevel);
        }

        private static string GetDescription(int index, int cartLevel)
        {
            return ResourceManager.Instance.LocalizationResources.Player.CartUpkeep(index, cartLevel);
        }
    }
}