namespace Data.Modifiable
{
    /// <summary>
    /// For cases that are not displayed in the UI.
    /// </summary>
    public sealed class GenericBaseValueModifier : BaseValueModifier
    {
        public GenericBaseValueModifier(float value) : base(value, "YOU'RE NOT SUPPOSED TO SEE THIS")
        {
        }
    }
}