namespace Data.Modifiable
{
    /// <summary>
    /// For cases that are not displayed in the UI.
    /// </summary>
    public sealed class GenericBasePercentageModifier : BasePercentageModifier
    {
        public GenericBasePercentageModifier(float value) : base(value, "YOU'RE NOT SUPPOSED TO SEE THIS")
        {
        }
    }
}