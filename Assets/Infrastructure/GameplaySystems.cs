using Common;
using Features.Levels.Logic;
using Features.Tutorial;

namespace Infrastructure
{
    public sealed class GameplaySystems
    {
        public DividendsSystem DividendsSystem { get; } = new();
        public LevelConditionManager LevelConditionManager { get; } = new();
        
        public void Initialize()
        {
            DividendsSystem.Initialize();
            LevelConditionManager.Initialize();
        }

        public void CleanUp()
        {
            DividendsSystem.CleanUp();
            LevelConditionManager.CleanUp();
        }
    }
}