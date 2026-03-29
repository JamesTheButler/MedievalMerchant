using Common.Infrastructure.Observation;
using Features.Map.Pathfinding;
using UnityEngine;

namespace Features.Player.Logic
{
    public sealed class PlayerLocation : IMapEntity
    {
        public Observable<Vector2> WorldLocation { get; } = new();
        public Observable<IMapLocation> MapLocation { get; } = new();
    }
}
