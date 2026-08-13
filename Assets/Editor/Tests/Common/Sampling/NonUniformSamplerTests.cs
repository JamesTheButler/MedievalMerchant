using Common.Config.Sampling;
using NUnit.Framework;
using UnityEngine;

namespace Editor.Tests.Common.Sampling
{
    public sealed class NonUniformSamplerTests
    {
        private static AnimationCurve SinglePeakCurve()
        {
            return new AnimationCurve(
                new Keyframe(0.5f, 0.1f),
                new Keyframe(1.15f, 1f),
                new Keyframe(2f, 0.1f));
        }

        [Test]
        public void Sample_StaysWithinRange()
        {
            var sampler = new NonUniformSampler(SinglePeakCurve(), 0.5f, 2f);

            for (var i = 0; i < 2000; i++)
            {
                var value = sampler.Sample();
                Assert.GreaterOrEqual(value, 0.5f);
                Assert.LessOrEqual(value, 2f);
            }
        }

        [Test]
        public void Sample_RespectsCurveShape_FavorsPeakBucket()
        {
            var sampler = new NonUniformSampler(SinglePeakCurve(), 0.5f, 2f);
            const int bucketCount = 6;
            var buckets = new int[bucketCount];

            for (var i = 0; i < 6000; i++)
            {
                var value = sampler.Sample();
                var t = Mathf.InverseLerp(0.5f, 2f, value);
                var bucket = Mathf.Clamp(Mathf.FloorToInt(t * bucketCount), 0, bucketCount - 1);
                buckets[bucket]++;
            }

            var peakBucket = Mathf.Clamp(
                Mathf.FloorToInt(Mathf.InverseLerp(0.5f, 2f, 1.15f) * bucketCount),
                0, bucketCount - 1);

            Assert.Greater(buckets[peakBucket], buckets[0] * 2);
        }

        [Test]
        public void Sample_EmptyCurve_FallsBackToUniformRange()
        {
            var sampler = new NonUniformSampler(new AnimationCurve(), 1f, 2f);

            Assert.DoesNotThrow(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    var value = sampler.Sample();
                    Assert.GreaterOrEqual(value, 1f);
                    Assert.LessOrEqual(value, 2f);
                }
            });
        }

        [Test]
        public void Sample_FlatZeroCurve_FallsBackToUniformRange()
        {
            var flatZero = new AnimationCurve(new Keyframe(1f, 0f), new Keyframe(2f, 0f));
            var sampler = new NonUniformSampler(flatZero, 1f, 2f);

            Assert.DoesNotThrow(() =>
            {
                for (var i = 0; i < 100; i++)
                {
                    var value = sampler.Sample();
                    Assert.GreaterOrEqual(value, 1f);
                    Assert.LessOrEqual(value, 2f);
                }
            });
        }
    }
}
