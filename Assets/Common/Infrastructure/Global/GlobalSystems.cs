using System.Collections.Generic;
using Features.Settings.Logic;

namespace Common.Infrastructure.Global
{
    public sealed class GlobalSystems : IInitializable
    {
        private readonly List<ISystem> _systems = new();

        public void Initialize()
        {
            _systems.Add(new AudioSettingsAutoSaveSystem());

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
        }
    }
}