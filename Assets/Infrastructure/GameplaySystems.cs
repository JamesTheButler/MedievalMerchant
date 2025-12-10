using System.Collections.Generic;
using Common;
using Features.Levels.Logic;
using Features.Player;
using Features.Ticking;

namespace Infrastructure
{
    public sealed class GameplaySystems
    {
        // TODO - cleanup: this should not be here
        public LevelConditionManager LevelConditionManager { get; } = new();

        private List<ISystem> _systems;

        public void Initialize()
        {
            _systems = new List<ISystem>
            {
                new DividendsSystem(),
                new PlayerTickSystem(),
                new DateSystem(),
                new PlayerUpkeepSystem(),
                LevelConditionManager
            };

            foreach (var system in _systems)
            {
                system.Initialize();
            }
        }

        public void CleanUp()
        {
            foreach (var system in _systems)
            {
                system.CleanUp();
            }

            _systems.Clear();
        }
    }
}