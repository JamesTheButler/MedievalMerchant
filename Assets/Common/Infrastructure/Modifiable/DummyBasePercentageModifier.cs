namespace Common.Infrastructure.Modifiable
{
    public sealed class DummyBasePercentageModifier : BasePercentageModifier
    {
        public DummyBasePercentageModifier(float value) : base(value, "Dummy base percentage value for testing.") { }
    }
}