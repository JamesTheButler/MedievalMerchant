using Common.Infrastructure.Observation;
using Features.Map.Pathfinding;
using Features.Towns;
using UnityEngine;

namespace Features.Player.Logic
{
    public sealed class PlayerLocation : IMapLocation
    {
        public Observable<Vector2> WorldLocation { get; } = new();
        public Observable<Town> CurrentTown { get; } = new();
    }
}