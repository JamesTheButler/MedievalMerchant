using Common.Infrastructure;
using Common.Infrastructure.Observation;
using Features.Map.Pathfinding;

namespace Features.Map
{
    public sealed class NavigationService : IService
    {
        public ObservableEvent<IMapLocation> NavigationStarted { get; } = new();

        public void Initialize() { }
        public void CleanUp() { }
    }
}
