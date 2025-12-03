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
        private Configurations configurations;

        private void Awake()
        {
            if (Configurations != null && Configurations != configurations)
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

            Configurations = configurations;
        }
    }
}