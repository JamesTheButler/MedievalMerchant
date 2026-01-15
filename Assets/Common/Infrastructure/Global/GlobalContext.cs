using Features.Levels;
using JetBrains.Annotations;
using UnityEngine;

namespace Common.Infrastructure.Global
{
    public sealed class GlobalContext : MonoBehaviour
    {
        public static GlobalContext Instance { get; private set; }

        public PersistenceServices PersistenceServices { get; } = new();
        public GlobalServices Services { get; } = new();
        public GlobalModel Model { get; } = new();
        public GlobalSystems Systems { get; } = new();

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
            PersistenceServices.Initialize();
            Model.Initialize();
            Services.Initialize();
            Systems.Initialize();
        }
    }
}