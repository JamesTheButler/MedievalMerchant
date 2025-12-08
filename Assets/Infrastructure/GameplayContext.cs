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

        public GameplayModel Model { get; private set; } = new();
        public GameplaySystems Systems { get; private set; } = new();
        public Selection Selection { get; private set; } = new();
        public GameplayServices Services { get; private set; } = new();

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