using System;
using UnityEngine;

namespace Common.Config.Sampling
{
    [Serializable]
    public sealed class UniformSampler : ISampler
    {
        [field: SerializeField]
        public float Minimum { get; private set; }

        [field: SerializeField]
        public float Maximum { get; private set; } = 1f;

        public UniformSampler() { }

        public UniformSampler(float minimum, float maximum)
        {
            Minimum = minimum;
            Maximum = maximum;
        }

        public float Sample()
        {
            return UnityEngine.Random.Range(Minimum, Maximum);
        }
    }
}
