using Common.Infrastructure.Observation;
using UnityEngine;

namespace Features.Map.Pathfinding
{
    public interface IMapEntity
    {
        Observable<Vector2> WorldLocation { get; }
        Observable<IMapLocation> MapLocation { get; }
    }
}
