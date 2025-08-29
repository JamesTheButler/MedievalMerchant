using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Art.Tiles
{
    [CreateAssetMenu(fileName = "RandomTileSet", menuName = "Data/RandomTileSet")]
    public class RandomTileSet : ScriptableObject
    {
        [SerializeField]
        private List<Tile> tileSet;

        public Tile GetRandom()
        {
            if (tileSet == null || tileSet.Count == 0) return null;

            return tileSet[Random.Range(0, tileSet.Count)];
        }
    }
}