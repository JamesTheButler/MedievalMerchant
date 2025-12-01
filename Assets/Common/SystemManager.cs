using System.Collections.Generic;
using Infrastructure;

namespace Common
{
    public sealed class SystemManager
    {
        private readonly List<ISystem> _services = new()
        {
            new DividendsSystem(),
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