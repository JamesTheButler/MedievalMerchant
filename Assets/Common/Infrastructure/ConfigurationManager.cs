using Common.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Common.Infrastructure
{
    [ExecuteInEditMode]
    public sealed class ConfigurationManager : MonoBehaviour
    {
        private static ConfigurationManager _instance;
        public static Configurations Configurations;

        [SerializeField, Required, Expandable]
        private Configurations debugConfigs, releaseConfigs;

        private void Awake()
        {
            if (_instance != null && _instance != this)
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

            _instance = this;
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