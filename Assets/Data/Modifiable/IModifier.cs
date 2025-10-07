namespace Data.Modifiable
{
    public interface IModifier
    {
        // TODO: public event Action ValueChanged;

        public float Value { get; }
        public string Description { get; }

        public string ToDisplayString();
    }
}