using System.Collections.Generic;
using Common;
using UnityEngine;

namespace Features.Map.Tiling
{
    public sealed class TileFlagMap
    {
        public Vector2Int Origin { get; }
        public Vector2Int Size { get; }

        private readonly TileFlags[,] _tiles;
        public readonly Dictionary<Vector2Int, int> TownZLevels = new();

        public TileFlagMap(Vector2Int size, Vector2Int origin)
        {
            Size = size;
            Origin = origin;
            _tiles = new TileFlags[size.x, size.y];
        }

        public bool HasTile(Vector2Int position, TileType tileType)
        {
            return _tiles[position.x - Origin.x, position.y - Origin.y].Has(tileType);
        }

        public void AddType(Vector2Int position, TileType tileType)
        {
            _tiles[position.x - Origin.x, position.y - Origin.y].AddType(tileType);
        }

        public void AddTown(Vector3Int position)
        {
            TownZLevels.Add(position.XY() - Origin, position.z);
        }
    }
}