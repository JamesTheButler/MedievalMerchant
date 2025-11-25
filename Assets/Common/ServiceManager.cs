using System.Collections.Generic;
using Features.Player;

namespace Common
{
    public sealed class ServiceManager
    {
        private readonly List<IService> _services = new()
        {
            new DividendsService(),
        };

        public void Initialize()
        {
            foreach (var service in _services)
            {
                service.Initialize();
            }
        }

        // TODO - STYLE: this is never called
        public void CleanUp()
        {
            foreach (var service in _services)
            {
                service.CleanUp();
            }
        }
    }
}