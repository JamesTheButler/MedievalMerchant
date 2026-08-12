namespace Common.Infrastructure.Observation
{
    public class Observable<T> : ReadOnlyObservable<T>
    {
        public new T Value
        {
            get => base.Value;
            set => SetValue(value);
        }

        public Observable(T value = default) : base(value) { }
    }
}