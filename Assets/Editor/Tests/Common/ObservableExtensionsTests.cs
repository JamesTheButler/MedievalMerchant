using Common.Infrastructure.Observation;
using NUnit.Framework;

namespace Editor.Tests.Common
{
    public sealed class ObservableExtensionsTests
    {
        [Test]
        public void Transform_AppliesTransformToCurrentSourceValue()
        {
            var source = new Observable<int>(3);

            var transformed = source.Transform(value => value * 2);

            Assert.AreEqual(6, transformed.Value);
        }

        [Test]
        public void Transform_SourceChange_UpdatesTransformedValue()
        {
            var source = new Observable<int>(3);
            var transformed = source.Transform(value => value * 2);

            source.Value = 5;

            Assert.AreEqual(10, transformed.Value);
        }

        [Test]
        public void Transform_SupportsDifferingInputAndOutputTypes()
        {
            var source = new Observable<int>(3);
            var transformed = source.Transform(value => $"#{value}");

            source.Value = 7;

            Assert.AreEqual("#7", transformed.Value);
        }

        [Test]
        public void Transform_AcceptsReadOnlyObservableSourceDirectly()
        {
            var source = new Observable<int>(4);
            ReadOnlyObservable<int> readOnlySource = source;

            var transformed = readOnlySource.Transform(value => value + 1);

            Assert.AreEqual(5, transformed.Value);
        }

        [Test]
        public void Transform_CanChainOffAnotherTransformedResult()
        {
            var source = new Observable<int>(2);
            var doubled = source.Transform(value => value * 2);
            var stringified = doubled.Transform(value => $"[{value}]");

            source.Value = 5;

            Assert.AreEqual("[10]", stringified.Value);
        }

        [Test]
        public void Invert_Int_NegatesCurrentValue()
        {
            var source = new Observable<int>(3);

            var inverted = source.Invert();

            Assert.AreEqual(-3, inverted.Value);
        }

        [Test]
        public void Invert_Int_UpdatesOnSourceChange()
        {
            var source = new Observable<int>(3);
            var inverted = source.Invert();

            source.Value = -8;

            Assert.AreEqual(8, inverted.Value);
        }

        [Test]
        public void Invert_Float_NegatesCurrentValue()
        {
            var source = new Observable<float>(2.5f);

            var inverted = source.Invert();

            Assert.AreEqual(-2.5f, inverted.Value);
        }

        [Test]
        public void Invert_Float_UpdatesOnSourceChange()
        {
            var source = new Observable<float>(2.5f);
            var inverted = source.Invert();

            source.Value = -1.5f;

            Assert.AreEqual(1.5f, inverted.Value);
        }
    }
}
