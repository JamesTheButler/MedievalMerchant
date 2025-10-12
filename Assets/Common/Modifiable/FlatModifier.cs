namespace Common.Modifiable
{
    public abstract class FlatModifier : IModifier
    {
        public float Value { get; }
        public string Description { get; }

        protected FlatModifier(float value, string description)
        {
            Value = value;
            Description = description;
        }

        public string ToDisplayString()
        {
            return $"{Value.Sign()}{Value:0.##} .. {Description}";
        }
    }
}