using System.Collections.Generic;
using Common.Utility;
using UnityEngine;

namespace Features.Map.Tiling
{
    public sealed class TileFlagMap
    {
        public Vector2Int Origin { get; }
        public Vector2Int Size { get; }

        private readonly TileFlags[,] _tiles;
        public readonly Dictionary<Vector2Int, int> TownZLevels = new();
        public readonly Dictionary<Vector2Int, TownMapTile> TownTiles = new();
        public readonly Dictionary<Vector2Int, int> CampZLevels = new();
        public readonly Dictionary<Vector2Int, CampMapTile> CampTiles = new();

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

        public void AddTown(Vector3Int position, TownMapTile mapTile)
        {
            var townPosition = position.XY() - Origin;
            TownZLevels.Add(townPosition, position.z);
            TownTiles.Add(position.XY(), mapTile);
        }

        public void AddCamp(Vector3Int position, CampMapTile mapTile)
        {
            var campPosition = position.XY() - Origin;
            CampZLevels.Add(campPosition, position.z);
            CampTiles.Add(position.XY(), mapTile);
        }
    }
}