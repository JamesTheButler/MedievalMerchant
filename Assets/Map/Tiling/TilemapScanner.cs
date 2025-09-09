using System.Collections.Generic;
using Common;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Map.Tiling
{
    public static class TilemapScanner
    {
        public static TileFlagMap Scan(Tilemap tilemap)
        {
            tilemap.CompressBounds();
            var size = tilemap.cellBounds.size; //- new Vector3Int(2, 2, 0); // subtract frame width
            var origin = tilemap.cellBounds.position; //- new Vector3Int(1, 1, 0);

            var flagMap = new TileFlagMap(size.XY(), origin.XY());

            for (var x = 0; x < size.x; x++)
            for (var y = 0; y < size.y; y++)
            for (var z = 0; z < size.z; z++)
            {
                var pos = new Vector3Int(x, y, z) + origin;
                var tileGameObject = tilemap.GetInstantiatedObject(pos);
                var tileInfo = tileGameObject?.GetComponent<TileInfo>();
                var tileType = tileInfo?.TileType;

                if (tileType == null) continue;

                flagMap.Tiles[x, y].AddType(tileType.Value);
            }

            return flagMap;
        }
    }
}