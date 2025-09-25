namespace Data.Trade.Price
{
    /// <summary>
    /// Modifiers that will be printed in the form "+/- 5% .. High Scarcity [icon]
    /// </summary>
    public abstract class PriceModifier
    {
        /// <summary>
        /// Modifier value in percent of the base price.
        /// </summary>
        public abstract float Value { get; }

        public abstract string Description { get; }

        public string ToDisplayString()
        {
            return $"{Value.Sign()}{Value * 100}% .. {Description}";
        }
    }
}