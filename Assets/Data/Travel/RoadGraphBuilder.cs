using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data.Travel
{
    public static class RoadGraphBuilder
    {
        private const int RoadZIndex = 1;

        private static readonly Vector3Int[] Dirs4 =
        {
            Vector3Int.right,
            Vector3Int.up,
            Vector3Int.left,
            Vector3Int.down
        };

        public static RoadGraph Build(Tilemap roads)
        {
            var graph = new RoadGraph();
            var bounds = roads.cellBounds;

            // Collect all road tiles as nodes
            for (var x = bounds.xMin; x < bounds.xMax; x++)
            {
                for (var y = bounds.yMin; y < bounds.yMax; y++)
                {
                    var tilePos = new Vector3Int(x, y, RoadZIndex);
                    if (roads.HasTile(tilePos))
                    {
                        //TileData t2 = new TileData();
                        //roads.GetTile(tilePos).GetTileData(tilePos, roads, ref t2);
                        //// t2.gameObject.
                        //graph.Nodes.Add(tilePos);
                    }
                }
            }

            // Wire neighbors
            foreach (var node in graph.Nodes)
            {
                var list = new List<Vector3Int>(4);
                foreach (var dir in Dirs4)
                {
                    var possibleNode = node + dir;
                    if (graph.Nodes.Contains(possibleNode)) list.Add(possibleNode);
                }

                graph.Neighbors[node] = list;
            }

            return graph;
        }
    }
}