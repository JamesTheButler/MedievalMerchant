using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using NUnit.Framework;

namespace Editor.Tests.Common
{
    public sealed class ModifiableVariableTests
    {
        private sealed class TestFlatModifier : FlatModifier
        {
            public TestFlatModifier(float value, string description = "flat") : base(value, description) { }
        }

        private sealed class TestPercentageModifier : BasePercentageModifier
        {
            public TestPercentageModifier(float value, string description = "percent") : base(value, description) { }
        }

        private sealed class TestBaseValueModifier : BaseValueModifier
        {
            public TestBaseValueModifier(float value, string description = "base") : base(value, description) { }
        }

        [Test]
        public void WithNoModifiers_ValueEqualsBaseValue()
        {
            var variable = new ModifiableVariable("test", true, new TestBaseValueModifier(10f));

            Assert.AreEqual(10f, variable.Value);
        }

        [Test]
        public void WithoutBaseValueModifier_ValueIsZero()
        {
            var variable = new ModifiableVariable("test", true);

            Assert.AreEqual(0f, variable.Value);
        }

        [Test]
        public void AddModifier_FlatModifier_AddsToValue()
        {
            var variable = new ModifiableVariable("test", true, new TestBaseValueModifier(10f));

            variable.AddModifier(new TestFlatModifier(5f));

            Assert.AreEqual(15f, variable.Value);
        }

        [Test]
        public void AddModifier_PercentageModifier_MultipliesValue()
        {
            var variable = new ModifiableVariable("test", true, new TestBaseValueModifier(10f));

            variable.AddModifier(new TestPercentageModifier(0.5f));

            Assert.AreEqual(15f, variable.Value);
        }

        [Test]
        public void AddModifier_FlatAndPercentage_AppliesFlatBeforePercentage()
        {
            var variable = new ModifiableVariable("test", true, new TestBaseValueModifier(10f));

            variable.AddModifier(new TestFlatModifier(10f));
            variable.AddModifier(new TestPercentageModifier(0.5f));

            Assert.AreEqual(30f, variable.Value);
        }

        [Test]
        public void RemoveModifier_RemovesItsContribution()
        {
            var variable = new ModifiableVariable("test", true, new TestBaseValueModifier(10f));
            var modifier = new TestFlatModifier(5f);
            variable.AddModifier(modifier);

            variable.RemoveModifier(modifier);

            Assert.AreEqual(10f, variable.Value);
        }

        [Test]
        public void ChangingBaseValue_UpdatesResultingValue()
        {
            var baseValue = new TestBaseValueModifier(10f);
            var variable = new ModifiableVariable("test", true, baseValue);
            variable.AddModifier(new TestFlatModifier(5f));

            baseValue.Value.Value = 20f;

            Assert.AreEqual(25f, variable.Value);
        }

        [Test]
        public void AddModifier_RaisesModifiersAddedAndModifiersChanged()
        {
            var variable = new ModifiableVariable("test", true, new TestBaseValueModifier(10f));
            IModifier added = null;
            var changedCount = 0;
            variable.ModifiersAdded += modifier => added = modifier;
            variable.ModifiersChanged += () => changedCount++;
            var modifier = new TestFlatModifier(5f);

            variable.AddModifier(modifier);

            Assert.AreEqual(modifier, added);
            Assert.AreEqual(1, changedCount);
        }

        [Test]
        public void Copy_MirrorsSubsequentModifierChanges()
        {
            var variable = new ModifiableVariable("test", true, new TestBaseValueModifier(10f));
            var copy = variable.Copy();

            variable.AddModifier(new TestFlatModifier(5f));

            Assert.AreEqual(variable.Value, copy.Value);
        }

        [Test]
        public void Modifiers_ExposesAddedModifiers()
        {
            var variable = new ModifiableVariable("test", true, new TestBaseValueModifier(10f));
            var modifier = new TestFlatModifier(5f);

            variable.AddModifier(modifier);

            CollectionAssert.Contains(variable.Modifiers, modifier);
        }
    }
}