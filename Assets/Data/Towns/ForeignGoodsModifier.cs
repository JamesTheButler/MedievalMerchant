namespace Data.Towns
{
    public sealed record ForeignGoodsModifier : IGrowthModifier
    {
        public float Value { get; }
        public string Description { get; }

        public ForeignGoodsModifier(float modifiedValue, float goodCount, Tier producerTier)
        {
            Value = modifiedValue;
            Description = $"{goodCount} {producerTier} foreign goods in storage.";
        }
    }
}