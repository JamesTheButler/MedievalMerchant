using Common;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Features.Map.Tiling
{
    public static class TilemapScanner
    {
        public static TileFlagMap Scan(Tilemap tilemap)
        {
            tilemap.CompressBounds();
            var size = tilemap.cellBounds.size;
            var origin = tilemap.cellBounds.position;

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

                flagMap.AddType(pos.XY(), tileType.Value);
                if(tileType == TileType.Town)
                {
                    flagMap.AddTown(pos);
                }
            }

            return flagMap;
        }
    }
}