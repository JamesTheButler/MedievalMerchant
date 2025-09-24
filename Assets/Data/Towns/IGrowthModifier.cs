namespace Data.Towns
{
    public interface IGrowthModifier
    {
        public float Value { get; }
        public string Description { get; }
    }
}