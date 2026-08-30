namespace Common.Infrastructure.Observation
{
    /// <summary>
    /// Represents an Observable that is a result of one/many source observables.
    /// Captures <see cref="IBinding"/>s of the sources to unbind when this <see cref="DerivedObservable{T}"/> is being unbound from.
    /// </summary>
    public sealed class DerivedObservable<T> : Observable<T>, IBinding
    {
        private readonly Bindings _sourceBindings = new();

        public DerivedObservable(T value) : base(value) { }

        public void TrackSource(params IBinding[] sources)
        {
            _sourceBindings.Track(sources);
        }

        public void Unbind()
        {
            _sourceBindings.Unbind();
        }
    }
}