namespace Data.Modifiable
{
    public interface IModifier
    {
        public float Value { get; }
        public string Description { get; }

        public string ToDisplayString();
    }
}