using System;

namespace Common.Infrastructure.Observation
{
    public static class ObservableExtensions
    {
        public static DerivedObservable<TOut> Transform<TIn, TOut>(
            this ReadOnlyObservable<TIn> observable,
            Func<TIn, TOut> transform)
        {
            var result = new DerivedObservable<TOut>(transform.Invoke(observable.Value));
            result.TrackSource(observable.Observe(value => result.Value = transform.Invoke(value), false));
            return result;
        }

        public static DerivedObservable<int> Invert(this ReadOnlyObservable<int> observable)
        {
            return observable.Transform(value => -value);
        }

        public static DerivedObservable<float> Invert(this ReadOnlyObservable<float> observable)
        {
            return observable.Transform(value => -value);
        }

        public static DerivedObservable<TOut> Combine<TIn1, TIn2, TOut>(
            ReadOnlyObservable<TIn1> in1,
            ReadOnlyObservable<TIn2> in2,
            Func<TIn1, TIn2, TOut> combine)
        {
            var result = new DerivedObservable<TOut>(combine.Invoke(in1.Value, in2.Value));

            result.TrackSource(
                in1.Observe(value1 => result.Value = combine.Invoke(value1, in2.Value), false),
                in2.Observe(value2 => result.Value = combine.Invoke(in1.Value, value2), false)
            );

            return result;
        }
    }
}