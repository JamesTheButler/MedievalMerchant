using Common;
using Features.Levels.Config;
using JetBrains.Annotations;

namespace Infrastructure
{
    /// <summary>
    /// Manages global models and services.
    /// </summary>
    public static class GlobalContext
    {
        public static GlobalServices Services { get; } = new();
        public static ProgressModel ProgressModel { get; } = new();
        
        [CanBeNull]
        public static LevelInfo CurrentLevelInfo { get; set; } 
    }
}