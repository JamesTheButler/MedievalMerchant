using System.Collections.Generic;
using Art.Tiles;
using Data;
using Data.Towns;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
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
    private RandomTileSet townTileSetT1;

    [SerializeField]
    private RandomTileSet townTileSetT2;

    [SerializeField]
    private RandomTileSet townTileSetT3;

    [SerializeField]
    private Tile selectionTile;

    [SerializeField]
    private UnityEvent<Town> onTownClicked;

    [SerializeField]
    private UnityEvent onGroundClicked;

    private Vector2Int? _selectedCoordinate;

    private const int GroundZIndex = 0;
    private const int TownZIndex = 5;
    private const int SelectionZIndex = 10;

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
                    tilemap.SetTile(new Vector3Int(pos.x, pos.y, TownZIndex), townTileSetT1.GetRandom());
                }
            }
        }
    }

    private void UpdateTown(Town town)
    {
        var tileSet = town.Tier switch
        {
            Tier.Tier1 => townTileSetT1,
            Tier.Tier2 => townTileSetT2,
            Tier.Tier3 => townTileSetT3,
            _ => townTileSetT3
        };

        var pos = _origin + town.Location;
        tilemap.SetTile(new Vector3Int(pos.x, pos.y, TownZIndex), tileSet.GetRandom());
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

        if (_selectedCoordinate != null)
        {
            var pos = new Vector3Int(_selectedCoordinate.Value.x, _selectedCoordinate.Value.y, SelectionZIndex);
            tilemap.SetTile(pos, null);
            _selectedCoordinate = null;
        }

        if (Model.Instance.Towns.TryGetValue(cellPos, out var town))
        {
            var pos = cellPos + _origin;
            _selectedCoordinate = pos;
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, SelectionZIndex) , selectionTile) ;
            onTownClicked?.Invoke(town);
        }
        else
        {
            onGroundClicked?.Invoke();
        }
    }
}