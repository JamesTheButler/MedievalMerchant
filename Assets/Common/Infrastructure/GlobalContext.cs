using Features.Levels.Config;
using JetBrains.Annotations;
using UnityEngine;

namespace Common.Infrastructure
{
    /// <summary>
    /// Manages global models and services.
    /// </summary>
    public sealed class GlobalContext : MonoBehaviour
    {
        public static GlobalContext Instance { get; private set; }

        public GlobalServices Services { get; private set; }
        public ProgressModel ProgressModel { get; private set; }

        [CanBeNull]
        public static LevelInfo CurrentLevelInfo { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);

            Initialize();
        }

        private void Initialize()
        {
            Services = new GlobalServices();
            ProgressModel = new ProgressModel();

            Services.Initialize();
            ProgressModel.Initialize();
        }
    }
}