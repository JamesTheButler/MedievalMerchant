using System.Collections.Generic;
using UnityEngine;

namespace Features.Map.Pathfinding
{
    public static class AStar
    {
        public static bool FindPath(RoadGraph g, Vector2Int start, Vector2Int goal, out List<Vector2Int> path)
        {
            path = null;
            if (!g.IsNode(start) || !g.IsNode(goal)) return false;

            var open = new PriorityQueue<Vector2Int, float>();
            var cameFrom = new Dictionary<Vector2Int, Vector2Int>();
            var gScore = new Dictionary<Vector2Int, float> { [start] = 0f };

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

        private static float Heuristic(Vector2Int a, Vector2Int b)
        {
            return Mathf.Abs(a.x - b.x) + Mathf.Abs(a.y - b.y);
        }

        private static List<Vector2Int> Reconstruct(Dictionary<Vector2Int, Vector2Int> cameFrom, Vector2Int current)
        {
            var list = new List<Vector2Int> { current };
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