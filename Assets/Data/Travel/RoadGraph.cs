using System.Collections.Generic;
using UnityEngine;

namespace Data.Travel
{
    public sealed class RoadGraph
    {
        public readonly HashSet<Vector3Int> Nodes = new();
        public readonly Dictionary<Vector3Int, List<Vector3Int>> Neighbors = new();

        public bool IsNode(Vector3Int c) => Nodes.Contains(c);
        public IEnumerable<Vector3Int> GetNeighbors(Vector3Int c) => Neighbors.TryGetValue(c, out var list) ? list : System.Linq.Enumerable.Empty<Vector3Int>();
    }
}