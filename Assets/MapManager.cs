using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class MapManager : MonoBehaviour
{
    [SerializeField]
    private Tilemap tilemap;
    
    [SerializeField]
    private Tile grassTile;
    
    [SerializeField]
    private Tile townTile;

    public void PlaceTiles(int size, List<Vector2Int> townLocations)
    {
        for (var x = 0; x < size; x++)
        {
            for (var y = 0; y < size; y++)
            {
                var pos = new Vector3Int(x, y, 0);
                tilemap.SetTile(pos, grassTile);
            }
        }

        foreach (var townLocation in townLocations)
        {
            var pos = new Vector3Int(townLocation.x, townLocation.y, 1);
            tilemap.SetTile(pos, townTile);
        }
    }
}