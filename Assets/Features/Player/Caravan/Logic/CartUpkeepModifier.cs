using Common.Modifiable;

namespace Features.Player.Caravan.Logic
{
    public sealed class CartUpkeepModifier : FlatModifier
    {
        public CartUpkeepModifier(float value, int cartLevel) : base(value, GetDescription(cartLevel)) { }

        public void Update(float value, int cartLevel)
        {
            Value.Value = value;
            Description.Value = GetDescription(cartLevel);
        }

        private static string GetDescription(int cartLevel)
        {
            return $"Upkeep for Cart level {cartLevel}";
        }
    }
}