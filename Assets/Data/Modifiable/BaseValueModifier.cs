namespace Data.Modifiable
{
    public class BaseValueModifier : IModifier
    {
        public float Value { get; }
        public string Name { get; }
        public string Description { get; }

        public BaseValueModifier(float value, string name, string description)
        {
            Value = value;
            Name = name;
            Description = description;
        }

        public string ToDisplayString()
        {
            return $"{Value.Sign()}{Value} .. {Description}";
        }
    }
}