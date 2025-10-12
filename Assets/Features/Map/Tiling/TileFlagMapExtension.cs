using System.Collections.Generic;
using UnityEngine;

namespace Features.Map.Tiling
{
    public static class TileFlagMapExtension
    {
        public static List<Vector2Int> GetAllCells(this TileFlagMap map, TileType tileType)
        {
            var cells = new List<Vector2Int>();
            var min = map.Origin;
            var max = map.Origin + map.Size;

            for (var x = min.x; x < max.x; x++)
            for (var y = min.y; y < max.y; y++)
            {
                var xyPos = new Vector2Int(x, y);
                if (map.HasTile(xyPos, tileType))
                {
                    cells.Add(xyPos);
                }
            }

            return cells;
        }
    }
}