using System;
using UnityEngine;

namespace Common.Config.Sampling
{
    /// <summary>
    /// Samples a value in [Minimum, Maximum] from a designer-authored curve treated as an
    /// unnormalized PDF (x = output value, y = relative likelihood, peak ~= 1).
    /// The curve is baked into a cumulative-weight lookup table once, so Sample() is a fixed-cost
    /// binary search with no rejection loop. Falls back to a uniform roll if the curve is missing,
    /// has no keyframes, or integrates to ~0 total weight.
    /// </summary>
    [Serializable]
    public sealed class NonUniformSampler : ISampler
    {
        private const int DefaultResolution = 100;

        [field: SerializeField]
        public AnimationCurve Curve { get; private set; } = AnimationCurve.Constant(0f, 1f, 1f);

        [field: SerializeField]
        public float Minimum { get; private set; }

        [field: SerializeField]
        public float Maximum { get; private set; } = 1f;

        [field: SerializeField]
        public int Resolution { get; private set; } = DefaultResolution;

        [NonSerialized] private float[] _values;
        [NonSerialized] private float[] _cumulativeWeights;
        [NonSerialized] private float _totalWeight;
        [NonSerialized] private bool _isBaked;

        public NonUniformSampler() { }

        public NonUniformSampler(AnimationCurve curve, float minimum, float maximum, int resolution = DefaultResolution)
        {
            Curve = curve;
            Minimum = minimum;
            Maximum = maximum;
            Resolution = resolution;
        }

        public void Bake()
        {
            var sampleCount = Mathf.Max(Resolution, 2);
            _values = new float[sampleCount];
            _cumulativeWeights = new float[sampleCount];

            var runningWeight = 0f;
            for (var i = 0; i < sampleCount; i++)
            {
                var t = i / (float)(sampleCount - 1);
                var value = Mathf.Lerp(Minimum, Maximum, t);
                var weight = Curve == null ? 0f : Mathf.Max(0f, Curve.Evaluate(value));
                runningWeight += weight;
                _values[i] = value;
                _cumulativeWeights[i] = runningWeight;
            }

            _totalWeight = runningWeight;
            _isBaked = true;
        }

        public float Sample()
        {
            if (!_isBaked)
                Bake();

            if (_totalWeight <= Mathf.Epsilon)
                return UnityEngine.Random.Range(Minimum, Maximum);

            var roll = UnityEngine.Random.Range(0f, _totalWeight);
            var index = Array.BinarySearch(_cumulativeWeights, roll);
            if (index < 0)
                index = ~index;
            index = Mathf.Clamp(index, 0, _values.Length - 1);

            if (index == 0)
                return _values[0];

            var previousWeight = _cumulativeWeights[index - 1];
            var segmentWeight = _cumulativeWeights[index] - previousWeight;
            var segmentT = segmentWeight <= Mathf.Epsilon ? 0f : (roll - previousWeight) / segmentWeight;
            return Mathf.Lerp(_values[index - 1], _values[index], segmentT);
        }
    }
}
