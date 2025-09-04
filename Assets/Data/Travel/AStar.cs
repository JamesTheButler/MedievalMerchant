using System.Collections.Generic;
using UnityEngine;

namespace Data.Travel
{
    public static class AStar
    {
        public static bool FindPath(RoadGraph g, Vector3Int start, Vector3Int goal, out List<Vector3Int> path)
        {
            path = null;
            if (!g.IsNode(start) || !g.IsNode(goal)) return false;

            var open = new PriorityQueue<Vector3Int, float>();
            var cameFrom = new Dictionary<Vector3Int, Vector3Int>();
            var gScore = new Dictionary<Vector3Int, float> { [start] = 0f };

            open.Enqueue(start, Heuristic(start, goal));

            while (open.Count > 0)
            {
                var current = open.Dequeue();
                if (current == goal)
                {
                    path = Reconstruct(cameFrom, current);
                    return true;
                }

                foreach (var neighbor in g.GetNeighbors(current))
                {
                    var tentative = gScore[current] + 1f; // cost per tile (adjust if needed)
                    if (gScore.TryGetValue(neighbor, out var old) && !(tentative < old))
                        continue;

                    cameFrom[neighbor] = current;
                    gScore[neighbor] = tentative;
                    var f = tentative + Heuristic(neighbor, goal);
                    open.EnqueueOrUpdate(neighbor, f);
                }
            }

            return false;
        }

        private static float Heuristic(Vector3Int a, Vector3Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        private static List<Vector3Int> Reconstruct(Dictionary<Vector3Int, Vector3Int> cameFrom, Vector3Int current)
        {
            var list = new List<Vector3Int> { current };
            while (cameFrom.TryGetValue(current, out var prev))
            {
                current = prev;
                list.Add(current);
            }

            list.Reverse();
            return list;
        }
    }
}