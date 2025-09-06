using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.Pathfinding
{
    public sealed class RoadGraph
    {
        public HashSet<Vector2Int> Nodes { get; } = new();

        public readonly Dictionary<Vector2Int, List<Vector2Int>> Neighbors = new();

        public bool IsNode(Vector2Int coordinate) => Nodes.Contains(coordinate);

        public IEnumerable<Vector2Int> GetNeighbors(Vector2Int coordinate) =>
            Neighbors.TryGetValue(coordinate, out var list)
                ? list
                : Enumerable.Empty<Vector2Int>();
    }
}