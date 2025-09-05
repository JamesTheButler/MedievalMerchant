using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Map.Pathfinding
{
    public sealed class RoadGraph
    {
        public HashSet<Vector3Int> Nodes { get; } = new();

        public readonly Dictionary<Vector3Int, List<Vector3Int>> Neighbors = new();

        public bool IsNode(Vector3Int coordinate) => Nodes.Contains(coordinate);

        public IEnumerable<Vector3Int> GetNeighbors(Vector3Int coordinate) =>
            Neighbors.TryGetValue(coordinate, out var list)
                ? list
                : Enumerable.Empty<Vector3Int>();
    }
}