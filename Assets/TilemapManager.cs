using System.Collections.Generic;
using Data;
using UI;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

    [SerializeField]
    private InventoryUI inventoryUI;

    [SerializeField]
    private Tilemap tilemap;

    [SerializeField]
    private Grid grid;

    [SerializeField]
    private Tile grassTile;

    [SerializeField]
    private Tile townTileT1;

    [SerializeField]
    private Tile townTileT2;

    private int _size;
    private Vector2Int _origin;

    public void InitTilemap(int size, List<Vector2Int> townLocations)
    {
        _origin = new Vector2Int(-(size / 2), -(size / 2));

        for (var x = 0; x < size; x++)
        {
            for (var y = 0; y < size; y++)
            {
                var pos = _origin + new Vector2Int(x, y);
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, 0), grassTile);

                if (townLocations.Contains(new Vector2Int(x, y)))
                {
                    tilemap.SetTile(new Vector3Int(pos.x, pos.y, 1), townTileT1);
                }
            }
        }
    }

    public void UpdateTown(Vector2Int townLocation, Tier townTier)
    {
        var tile = townTier switch
        {
            Tier.Tier1 => townTileT1,
            Tier.Tier2 => townTileT2,
            _ => townTileT1
        };

        var pos = _origin + townLocation;
        tilemap.SetTile(new Vector3Int(pos.x, pos.y, 1), tile);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            LeftClick();
        }
    }

    private void LeftClick()
    {
        var mouseWorldPos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = grid.WorldToCell(mouseWorldPos).XY() - _origin;

        if (gameManager.Model.Towns.TryGetValue(cellPos, out var town))
        {
            var romanTier = ((int)town.Tier).ToRomanNumeral();
            inventoryUI.SetTitle($"{town.Name} ({romanTier})");
            inventoryUI.Bind(town.Inventory);
            inventoryUI.Show();
        }
        else
        {
            inventoryUI.UnBind();
            inventoryUI.Hide();
        }
    }
}