using System.Collections.Generic;
using Common;
using Features.Levels.Logic;
using Features.Player;
using Features.Ticking;
using Features.Towns;

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

            foreach (var town in GameplayContext.Instance.Model.Towns.Values)
            {
                _systems.Add(new TownTickSystem(town));
            }

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