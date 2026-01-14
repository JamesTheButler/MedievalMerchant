using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Features.Towns;

namespace Features.Map
{
    public sealed class NavigationService : IService
    {
        public ObservableEvent<Town> NavigationStarted { get; } = new();

        public void Initialize() { }
        public void CleanUp() { }
    }
}