using System.Collections.Generic;
using Features.Audio;
using Features.Levels.Conditions.Logic;
using Features.Levels.GameModifiers.Events;
using Features.Levels.Serialization;
using Features.Notifications.Logic;
using Features.Player.Logic;
using Features.Player.Retinue.Logic;
using Features.Stats;
using Features.Ticking.Logic;
using Features.Towns;
using Features.Towns.Development.Logic;
using Features.Towns.Development.Logic.Milestones;
using Features.Towns.Missions;
using Features.Towns.Production.Logic;
using Features.Tutorial.Logic;

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
            _systems.Add(new DateSystem());
            _systems.Add(new ConditionSystem());
            _systems.Add(new StatSystem());
            _systems.Add(new EventSystem());
            _systems.Add(new TutorialSystem());
            _systems.Add(new NotificationPingSystem());
            _systems.Add(new NotificationLoggerSystem());
            _systems.Add(new ProgressionSystem());
            _systems.Add(new GameSfxSystem());
        }

        private void AddPlayerSystems()
        {
            _systems.Add(new PlayerTickSystem());
            _systems.Add(new PlayerUpkeepSystem());
            _systems.Add(new PlayerTradeTrackingSystem());
            _systems.Add(new RetinueSystem());
            _systems.Add(new PlayerMapSpeedSystem());
            _systems.Add(new PlayerInTownPauseSystem());
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
                _systems.Add(new DevelopmentMilestoneSystem(town));
                _systems.Add(new MilestoneModifierSystem(town));
                _systems.Add(new MissionSystem(town));
                //_systems.Add(new TownNeglectSystem(town)); // TODO - Milestone 0.2.0
            }
        }
    }
}