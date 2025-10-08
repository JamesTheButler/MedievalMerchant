namespace Data.Modifiable
{
    public interface IModifier
    {
        // TODO - CORE: use ModifiableVariable instead
        public float Value { get; }
        public string Description { get; }

        public string ToDisplayString();
    }
}