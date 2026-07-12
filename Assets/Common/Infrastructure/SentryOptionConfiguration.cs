using Sentry.Unity;
using UnityEngine;

namespace Common.Infrastructure
{
    [CreateAssetMenu(fileName = nameof(SentryOptionConfiguration))]
    public sealed class SentryOptionConfiguration : SentryOptionsConfiguration
    {
        public override void Configure(SentryUnityOptions options)
        {
#if UNITY_EDITOR
            options.Enabled = false;
#else
        options.Enabled = true;
#endif
        }
    }
}