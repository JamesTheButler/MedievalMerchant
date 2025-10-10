namespace Data.Modifiable
{
    public sealed class AverageBaseValueModifier : BaseValueModifier
    {
        public AverageBaseValueModifier(float value, string propertyName) : base(value, $"Average {propertyName}")
        {
        }
    }
}