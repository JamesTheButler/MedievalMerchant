using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using NUnit.Framework;

namespace Editor.Tests.Common
{
    public sealed class ObservableSumTests
    {
        [Test]
        public void DefaultConstructor_StartsAtZero()
        {
            var sum = new ObservableSum();

            Assert.AreEqual(0f, sum.Value);
        }

        [Test]
        public void ConstructorWithValues_SumsInitialValues()
        {
            var sum = new ObservableSum(new[]
            {
                new Observable<float>(2f),
                new Observable<float>(3f),
                new Observable<float>(4f)
            });

            Assert.AreEqual(9f, sum.Value);
        }

        [Test]
        public void AddValue_IncludesValueInSum()
        {
            var sum = new ObservableSum();
            var first = new Observable<float>(2f);
            var second = new Observable<float>(5f);

            sum.AddValue(first);
            sum.AddValue(second);

            Assert.AreEqual(7f, sum.Value);
        }

        [Test]
        public void ChangingAddedValue_UpdatesSum()
        {
            var sum = new ObservableSum();
            var component = new Observable<float>(2f);
            sum.AddValue(component);

            component.Value = 10f;

            Assert.AreEqual(10f, sum.Value);
        }

        [Test]
        public void RemoveValue_ExcludesValueFromSum()
        {
            var sum = new ObservableSum();
            var first = new Observable<float>(2f);
            var second = new Observable<float>(5f);
            sum.AddValue(first);
            sum.AddValue(second);

            sum.RemoveValue(first);

            Assert.AreEqual(5f, sum.Value);
        }

        [Test]
        public void RemoveValue_StopsTrackingFurtherChanges()
        {
            var sum = new ObservableSum();
            var component = new Observable<float>(2f);
            sum.AddValue(component);

            sum.RemoveValue(component);
            component.Value = 100f;

            Assert.AreEqual(0f, sum.Value);
        }

        [Test]
        public void Value_IsSumOfAddedObservables()
        {
            var sum = new ObservableSum();
            sum.AddValue(new Observable<float>(2f));
            sum.AddValue(new Observable<float>(3f));

            Assert.AreEqual(5f, sum.Value);
        }

        [Test]
        public void Value_UpdatesWhenAddedObservableChanges()
        {
            var sum = new ObservableSum();
            var tracked = new Observable<float>(1f);
            sum.AddValue(tracked);

            tracked.Value = 4f;

            Assert.AreEqual(4f, sum.Value);
        }

        [Test]
        public void Value_DecreasesWhenObservableIsRemoved()
        {
            var first = new Observable<float>(2f);
            var second = new Observable<float>(3f);
            var sum = new ObservableSum(new[] { first, second });

            sum.RemoveValue(first);

            Assert.AreEqual(3f, sum.Value);
        }
    }
}