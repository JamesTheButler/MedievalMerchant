using System.Collections.Generic;
using System.Linq;
using Data.Towns;
using Tilemap;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Data.Travel
{
    public sealed class WorldScanner : MonoBehaviour
    {
        [field: SerializeField]
        public UnityEngine.Tilemaps.Tilemap Tilemap { get; private set; }

        public List<Town> ScanTownsAndBiomes()
        {
            var towns = new List<Town>();
            var townTiles =
                FindObjectsByType<TileInfo>(FindObjectsInactive.Exclude, FindObjectsSortMode.None)
                    .Where(info => info.TileType == TileType.Town);
            foreach (var townTile in townTiles) // place Town components in scene
            {
                var cell = Tilemap.WorldToCell(townTile.transform.position);
                //var town = new Town(cell, )
            }

            return towns;
        }
    }
}