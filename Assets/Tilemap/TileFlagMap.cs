using UnityEngine;

namespace Tilemap
{
    public sealed class TileFlagMap
    {
        public Vector2Int Origin { get; }
        public Vector2Int Size { get; }
        public TileFlags[,] Tiles { get; }

        public TileFlagMap(Vector2Int size, Vector2Int origin)
        {
            Size = size;
            Origin = origin;
            Tiles = new TileFlags[size.x, size.y];
        }
    }
}