using System.Collections.Generic;
using Features.Map.Tiling;
using UnityEngine;

namespace Features.Map.Pathfinding
{
    public static class RoadGraphBuilder
    {
        private static readonly Vector2Int[] Dirs4 =
        {
            Vector2Int.right,
            Vector2Int.up,
            Vector2Int.left,
            Vector2Int.down
        };

        public static RoadGraph Build(TileFlagMap flagMap)
        {
            var graph = new RoadGraph();
            var roadTiles = flagMap.GetAllCells(TileType.Road);
            foreach (var roadTile in roadTiles)
            {
                graph.Nodes.Add(roadTile);
            }
            
            // Wire neighbors
            foreach (var node in graph.Nodes)
            {
                var list = new List<Vector2Int>(4);
                foreach (var dir in Dirs4)
                {
                    var neighborPosition = node + dir;
                    if (graph.Nodes.Contains(neighborPosition))
                    {
                        list.Add(neighborPosition);
                    }
                }

                graph.Neighbors[node] = list;
            }

            return graph;
        }
    }
}