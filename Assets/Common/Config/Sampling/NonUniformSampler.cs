using System;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Common.Config.Sampling
{
    [Serializable]
    public sealed class NonUniformSampler : ISampler
    {
        [SerializeField]
        private AnimationCurve cdf;

        [SerializeField]
        private float min, max;

        public float Sample()
        {
            var u = Random.value;
            var t = cdf.Evaluate(u); // 0..1
            return Mathf.Lerp(min, max, t);
        }
    }
}