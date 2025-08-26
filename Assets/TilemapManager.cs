using System.Collections.Generic;
using Data;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public sealed class TilemapManager : MonoBehaviour
{
    [SerializeField]
    private GameManager gameManager;

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

    [SerializeField]
    private Tile townTileT3;

    [SerializeField]
    private UnityEvent<Town> onTownClicked;

    [SerializeField]
    private UnityEvent onGroundClicked;

    private const int GroundZIndex = 0;
    private const int TownZIndex = 5;

    private int _size;
    private Vector2Int _origin;

    private void Start()
    {
        foreach (var town in Model.Instance.Towns.Values)
        {
            town.TierChanged += () => UpdateTown(town);
        }
    }

    public void InitTilemap(int size, List<Vector2Int> townLocations)
    {
        _origin = new Vector2Int(-(size / 2), -(size / 2));

        for (var x = 0; x < size; x++)
        {
            for (var y = 0; y < size; y++)
            {
                var pos = _origin + new Vector2Int(x, y);
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, GroundZIndex), grassTile);

                if (townLocations.Contains(new Vector2Int(x, y)))
                {
                    tilemap.SetTile(new Vector3Int(pos.x, pos.y, TownZIndex), townTileT1);
                }
            }
        }
    }

    private void UpdateTown(Town town)
    {
        var tile = town.Tier switch
        {
            Tier.Tier1 => townTileT1,
            Tier.Tier2 => townTileT2,
            Tier.Tier3 => townTileT3,
            _ => townTileT3
        };

        var pos = _origin + town.Location;
        tilemap.SetTile(new Vector3Int(pos.x, pos.y, TownZIndex), tile);
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject())
        {
            LeftClick();
        }
    }

    private void LeftClick()
    {
        var mouseWorldPos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        var cellPos = grid.WorldToCell(mouseWorldPos).XY() - _origin;

        if (Model.Instance.Towns.TryGetValue(cellPos, out var town))
        {
            onTownClicked?.Invoke(town);
        }
        else
        {
            onGroundClicked?.Invoke();
        }
    }
}