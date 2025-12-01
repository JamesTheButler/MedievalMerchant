using Common;
using Features.Towns;

namespace Infrastructure
{
    /// <summary>
    /// Manages models and services relevant while playing a specific level.
    /// </summary>
    public static class GameplayContext
    {
        public static GameplayModel Model { get; } = new();
        public static GameplaySystems Systems { get; } = new();
        public static Selection Selection { get; } = new();
    }
}