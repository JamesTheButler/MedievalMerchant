namespace Common.Modifiable
{
    public interface IModifier
    {
        // TODO - CORE: use ModifiableVariable instead
        public float Value { get; }

        public string FormattedValue { get; }
        public string Description { get; }
    }
}