using Common.Infrastructure.Modifiable;
using Common.Infrastructure.Observation;
using NUnit.Framework;

namespace Editor.Tests.Common
{
    public sealed class ObservableFilterTests
    {
        [Test]
        public void DefaultConstructor_StartsAtZero()
        {
            var filter = new ObservableFilter<int>(value => value > 5);

            Assert.AreEqual(0, filter.Value);
        }

        [Test]
        public void ConstructorWithValues_CountsMatchesAtConstruction()
        {
            var filter = new ObservableFilter<int>(new[]
            {
                new Observable<int>(1),
                new Observable<int>(10),
                new Observable<int>(20)
            }, value => value > 5);

            Assert.AreEqual(2, filter.Value);
        }

        [Test]
        public void AddValue_IncrementsCountOnlyWhenPredicateMatches()
        {
            var filter = new ObservableFilter<int>(value => value > 5);

            filter.AddValue(new Observable<int>(1));
            filter.AddValue(new Observable<int>(10));

            Assert.AreEqual(1, filter.Value);
        }

        [Test]
        public void ChangingTrackedValue_FromNonMatchingToMatching_IncrementsCount()
        {
            var filter = new ObservableFilter<int>(value => value > 5);
            var tracked = new Observable<int>(1);
            filter.AddValue(tracked);

            tracked.Value = 10;

            Assert.AreEqual(1, filter.Value);
        }

        [Test]
        public void ChangingTrackedValue_FromMatchingToNonMatching_DecrementsCount()
        {
            var filter = new ObservableFilter<int>(value => value > 5);
            var tracked = new Observable<int>(10);
            filter.AddValue(tracked);

            tracked.Value = 1;

            Assert.AreEqual(0, filter.Value);
        }

        [Test]
        public void ChangingTrackedValue_BetweenTwoMatchingValues_DoesNotChangeCount()
        {
            var filter = new ObservableFilter<int>(value => value > 5);
            var tracked = new Observable<int>(10);
            filter.AddValue(tracked);

            tracked.Value = 20;

            Assert.AreEqual(1, filter.Value);
        }

        [Test]
        public void RemoveValue_DecrementsCountWhenRemovedValueWasMatching()
        {
            var filter = new ObservableFilter<int>(value => value > 5);
            var tracked = new Observable<int>(10);
            filter.AddValue(tracked);

            filter.RemoveValue(tracked);

            Assert.AreEqual(0, filter.Value);
        }

        [Test]
        public void RemoveValue_StopsTrackingFurtherChanges()
        {
            var filter = new ObservableFilter<int>(value => value > 5);
            var tracked = new Observable<int>(10);
            filter.AddValue(tracked);
            filter.RemoveValue(tracked);

            tracked.Value = 999;

            Assert.AreEqual(0, filter.Value);
        }

        [Test]
        public void WithStringGenericParameter_CountsNonEmptyStrings()
        {
            var filter = new ObservableFilter<string>(value => !string.IsNullOrEmpty(value));
            var name = new Observable<string>(string.Empty);
            filter.AddValue(name);

            name.Value = "merchant";

            Assert.AreEqual(1, filter.Value);
        }

        [Test]
        public void Value_CountsObservablesMatchingPredicate()
        {
            var filter = new ObservableFilter<int>(value => value > 0);
            filter.AddValue(new Observable<int>(1));
            filter.AddValue(new Observable<int>(-1));
            filter.AddValue(new Observable<int>(2));

            Assert.AreEqual(2, filter.Value);
        }

        [Test]
        public void Value_UpdatesWhenObservableCrossesPredicateBoundary()
        {
            var filter = new ObservableFilter<int>(value => value > 0);
            var tracked = new Observable<int>(1);
            filter.AddValue(tracked);

            tracked.Value = -1;

            Assert.AreEqual(0, filter.Value);
        }

        [Test]
        public void Value_DecreasesWhenMatchingObservableIsRemoved()
        {
            var matching = new Observable<int>(1);
            var filter = new ObservableFilter<int>(new[] { matching }, value => value > 0);

            filter.RemoveValue(matching);

            Assert.AreEqual(0, filter.Value);
        }
    }
}