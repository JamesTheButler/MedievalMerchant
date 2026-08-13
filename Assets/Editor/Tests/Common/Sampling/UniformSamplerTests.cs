using Common.Config.Sampling;
using NUnit.Framework;

namespace Editor.Tests.Common.Sampling
{
    public sealed class UniformSamplerTests
    {
        [Test]
        public void Sample_StaysWithinRange()
        {
            var sampler = new UniformSampler(0.5f, 1.5f);

            for (var i = 0; i < 1000; i++)
            {
                var value = sampler.Sample();
                Assert.GreaterOrEqual(value, 0.5f);
                Assert.LessOrEqual(value, 1.5f);
            }
        }
    }
}
