using Common.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Common
{
    [ExecuteInEditMode]
    public sealed class ConfigurationManager : MonoBehaviour
    {
        public static Configurations Configurations;

        [SerializeField, Required, Expandable]
        private Configurations debugConfigs, releaseConfigs;

        private void Awake()
        {
            if (Configurations != null)
            {
                if (Application.isPlaying)
                {
                    Destroy(gameObject);
                }
                else
                {
                    DestroyImmediate(gameObject);
                }

                return;
            }

            if (Application.isPlaying)
            {
                DontDestroyOnLoad(gameObject);
            }

            Configurations = IsDebug() ? debugConfigs : releaseConfigs;
        }

        private static bool IsDebug()
        {
#if UNITY_EDITOR
            return true;
#else
            return false;
#endif
        }
    }
}