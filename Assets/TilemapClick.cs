using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapClick : MonoBehaviour
{
    public Grid grid;          // reference to your Grid
    public Tilemap tilemap;    // reference to your Tilemap

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // left click
        {
            var mouseWorldPos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
            var cellPos = grid.WorldToCell(mouseWorldPos);

            if (tilemap.HasTile(cellPos))
            {
                Debug.Log("Clicked tile at " + cellPos);
            }
        }
    }
}