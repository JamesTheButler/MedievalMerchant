using Common.Infrastructure.Observation;
using NUnit.Framework;

namespace Editor.Tests.Common
{
    public sealed class ObservableTests
    {
        [Test]
        public void DefaultConstructor_UsesDefaultValue()
        {
            var observable = new Observable<int>();

            Assert.AreEqual(0, observable.Value);
        }

        [Test]
        public void ConstructorWithValue_SetsInitialValue()
        {
            var observable = new Observable<string>("hello");

            Assert.AreEqual("hello", observable.Value);
        }

        [Test]
        public void SettingValue_UpdatesValue()
        {
            var observable = new Observable<int>(1);

            observable.Value = 5;

            Assert.AreEqual(5, observable.Value);
        }

        [Test]
        public void SettingValue_NotifiesValueChangedWithNewValue()
        {
            var observable = new Observable<int>(1);
            var received = -1;
            observable.Observe(value => received = value, invokeOnObserve: false);

            observable.Value = 7;

            Assert.AreEqual(7, received);
        }

        [Test]
        public void SettingValue_NotifiesValueChangedWithOldAndNewValue()
        {
            var observable = new Observable<string>("a");
            string oldReceived = null;
            string newReceived = null;
            observable.Observe((oldValue, newValue) =>
            {
                oldReceived = oldValue;
                newReceived = newValue;
            });

            observable.Value = "b";

            Assert.AreEqual("a", oldReceived);
            Assert.AreEqual("b", newReceived);
        }

        [Test]
        public void SettingValue_NotifiesParameterlessCallback()
        {
            var observable = new Observable<int>(1);
            var callCount = 0;
            observable.Observe(() => callCount++, invokeOnObserve: false);

            observable.Value = 2;

            Assert.AreEqual(1, callCount);
        }

        [Test]
        public void SettingSameValue_DoesNotNotify()
        {
            var observable = new Observable<int>(5);
            var callCount = 0;
            observable.Observe(_ => callCount++, invokeOnObserve: false);

            observable.Value = 5;

            Assert.AreEqual(0, callCount);
        }

        [Test]
        public void Observe_WithInvokeOnObserveTrue_FiresImmediately()
        {
            var observable = new Observable<int>(9);

            var received = -1;
            observable.Observe(value => received = value, invokeOnObserve: true);

            Assert.AreEqual(9, received);
        }

        [Test]
        public void Observe_WithInvokeOnObserveFalse_DoesNotFireImmediately()
        {
            var observable = new Observable<int>(9);

            var wasCalled = false;
            observable.Observe(_ => wasCalled = true, invokeOnObserve: false);

            Assert.IsFalse(wasCalled);
        }

        [Test]
        public void StopObserving_PreventsFurtherNotifications()
        {
            var observable = new Observable<int>(1);
            var callCount = 0;
            void Callback(int value) => callCount++;
            observable.Observe(Callback, invokeOnObserve: false);

            observable.StopObserving(Callback);
            observable.Value = 2;

            Assert.AreEqual(0, callCount);
        }

        [Test]
        public void UnbindingReturnedBinding_StopsNotifications()
        {
            var observable = new Observable<int>(1);
            var callCount = 0;
            var binding = observable.Observe(_ => callCount++, invokeOnObserve: false);

            binding.Unbind();
            observable.Value = 2;

            Assert.AreEqual(0, callCount);
        }

        [Test]
        public void ImplicitConversion_ReturnsValue()
        {
            var observable = new Observable<int>(42);

            int value = observable;

            Assert.AreEqual(42, value);
        }

        [Test]
        public void ReadOnlyView_ReflectsWritesMadeThroughObservable()
        {
            var observable = new Observable<int>(1);
            ReadOnlyObservable<int> readOnlyView = observable;

            observable.Value = 3;

            Assert.AreEqual(3, readOnlyView.Value);
        }

        [Test]
        public void ToString_WrapsValueInAngleBrackets()
        {
            var observable = new Observable<int>(3);

            Assert.AreEqual(">3<", observable.ToString());
        }
    }
}