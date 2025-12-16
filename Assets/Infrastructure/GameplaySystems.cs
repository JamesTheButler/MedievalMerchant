using System.Collections.Generic;
using Common;
using Features.Levels.Logic;
using Features.Player;
using Features.Ticking;
using Features.Towns;
using Features.Towns.Development.Logic;
using Features.Towns.Production.Logic;
using Features.Towns.Reputation.Logic;

namespace Infrastructure
{
    public sealed class GameplaySystems
    {
        // TODO - cleanup: this should not be here
        public LevelConditionManager LevelConditionManager { get; } = new();

        private readonly List<ISystem> _systems = new();

        public void Initialize()
        {
            AddGlobalSystems();
            AddTownSystems();

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

        private void AddGlobalSystems()
        {
            _systems.Add(new DividendsSystem());
            _systems.Add(new PlayerTickSystem());
            _systems.Add(new DateSystem());
            _systems.Add(new PlayerUpkeepSystem());
            _systems.Add(LevelConditionManager);
        }

        private void AddTownSystems()
        {
            var model = GameplayContext.Instance.Model;
            foreach (var town in model.Towns.Values)
            {
                _systems.Add(new TownFundsSystem(town));
                _systems.Add(new TownProductionSystem(town));
                _systems.Add(new TownDevelopmentSystem(town));
                _systems.Add(new TownConsumptionSystem(town));
                //_systems.Add(new TownNeglectSystem(town)); // TODO - Milestone 0.2.0
            }
        }
    }
}