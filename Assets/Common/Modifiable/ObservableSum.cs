namespace Common.Modifiable
{
    public sealed class ObservableSum : Observable<float>
    {
        public void AddValue(Observable<float> value)
        {
            value.Observe(Refresh);
            Value += value.Value;
        }

        public void RemoveValue(Observable<float> value)
        {
            value.StopObserving(Refresh);
            Value -= value.Value;
        }

        private void Refresh(float oldValue, float newValue)
        {
            Value = Value - oldValue + newValue;
        }
    }
}