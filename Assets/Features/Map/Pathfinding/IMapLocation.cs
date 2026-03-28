using Common.Infrastructure.Observation;
using Features.Towns;
using UnityEngine;

namespace Features.Map.Pathfinding
{
    public interface IMapLocation
    {
        Observable<Vector2> WorldLocation { get; }
        Observable<Town> CurrentTown { get; }
    }
}