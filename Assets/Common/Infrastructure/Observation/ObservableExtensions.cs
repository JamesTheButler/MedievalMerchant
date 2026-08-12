using System;

namespace Common.Infrastructure.Observation
{
    public static class ObservableExtensions
    {
        public static Observable<TOut> Transform<TIn, TOut>(this ReadOnlyObservable<TIn> observable, Func<TIn, TOut> transform)
        {
            var result = new Observable<TOut>(transform.Invoke(observable.Value));
            observable.Observe(value => result.Value = transform.Invoke(value));
            return result;
        }

        public static Observable<int> Invert(this ReadOnlyObservable<int> observable)
        {
            return observable.Transform(value => -value);
        }

        public static Observable<float> Invert(this ReadOnlyObservable<float> observable)
        {
            return observable.Transform(value => -value);
        }
    }
}