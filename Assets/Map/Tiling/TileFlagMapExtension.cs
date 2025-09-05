using System.Collections.Generic;
using UnityEngine;

namespace Map.Tiling
{
    public static class TileFlagMapExtension
    {
        public static List<Vector2Int> GetAllCells(this TileFlagMap map, TileType tileType)
        {
            var cells = new List<Vector2Int>();
            for (var x = 0; x < map.Size.x; x++)
            for (var y = 0; y < map.Size.y; y++)
            {
                if (map.Tiles[x, y].Has(tileType))
                {
                    cells.Add(new Vector2Int(x, y));
                }
            }

            return cells;
        }
    }
}