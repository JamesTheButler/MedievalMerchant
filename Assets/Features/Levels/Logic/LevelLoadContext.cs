using Features.Levels.Config;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.Logic
{
    public sealed class LevelLoadContext : MonoBehaviour
    {
        public static LevelLoadContext Instance;

        [field: SerializeField, ReadOnly]
        public LevelInfo SelectedLevel { get; set; }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}