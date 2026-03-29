using UnityEngine;

namespace Features.Map.Pathfinding
{
    public interface IMapLocation
    {
        Vector2Int GridLocation { get; }
        Vector2 WorldLocation { get; }
    }
}
