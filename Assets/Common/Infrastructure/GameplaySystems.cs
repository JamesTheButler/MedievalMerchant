using System.Collections.Generic;
using Features.Levels.Conditions.Logic;
using Features.Player;
using Features.Player.Logic;
using Features.Stats;
using Features.Ticking;
using Features.Towns;
using Features.Towns.Development.Logic;
using Features.Towns.Production.Logic;

namespace Common.Infrastructure
{
    public sealed class GameplaySystems
    {
        private readonly List<ISystem> _systems = new();

        public void Initialize()
        {
            AddGlobalSystems();
            AddPlayerSystems();
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
            _systems.Add(new DateSystem());
            _systems.Add(new ConditionSystem());
            _systems.Add(new StatSystem());
        }

        private void AddPlayerSystems()
        {
            _systems.Add(new PlayerTickSystem());
            _systems.Add(new PlayerUpkeepSystem());
            _systems.Add(new PlayerTradeTrackingSystem());
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