namespace Common.Modifiable
{
    public sealed class DummyBaseValueModifier : BaseValueModifier
    {
        public DummyBaseValueModifier(float value) : base(value, "Dummy base value for testing.") { }
    }
}