using Common;
using Features.Towns;
using UnityEngine;

namespace Infrastructure
{
    /// <summary>
    /// Manages models and services relevant while playing a specific level.
    /// </summary>
    public sealed class GameplayContext : MonoBehaviour
    {
        public static GameplayContext Instance { get; private set; }

        public GameplayModel Model { get; } = new();
        public GameplaySystems Systems { get; } = new();
        public Selection Selection { get; } = new();
        public GameplayServices Services { get; } = new();

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }

        private void OnDestroy()
        {
            Systems.CleanUp();
            Services.CleanUp();
        }
    }
}