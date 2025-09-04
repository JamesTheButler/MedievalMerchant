using System.Collections.Generic;
using Data;
using Data.Towns;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Tilemaps;

public sealed class TilemapManager : MonoBehaviour
{
    [SerializeField, Required]
    private GameManager gameManager;

    [SerializeField, Required]
    private Tilemap tilemap;

    [SerializeField, Required]
    private Grid grid;

    [SerializeField, Required]
    private Tiles tiles;

    [SerializeField]
    private UnityEvent<Town> onTownClicked;

    [SerializeField]
    private UnityEvent<float> onMapGenerated;

    [SerializeField]
    private UnityEvent onGroundClicked;

    private Vector2Int? _selectedCoordinate;

    private const int GroundZIndex = 0;
    private const int TownZIndex = 5;
    private const int SelectionZIndex = 10;
    private const int FrameZIndex = 15;

    private int _size;
    private Vector2Int _origin;

    private void Start()
    {
        foreach (var town in Model.Instance.Towns.Values)
        {
            town.TierChanged += _ => UpdateTown(town);
        }
    }

    public void InitTilemap(int size, List<Vector2Int> townLocations)
    {
        _origin = new Vector2Int(-(size / 2), -(size / 2));

        for (var x = -1; x < size + 1; x++)
        {
            for (var y = -1; y < size + 1; y++)
            {
                var pos = _origin + new Vector2Int(x, y);
                tilemap.SetTile(new Vector3Int(pos.x, pos.y, FrameZIndex), tiles.FrameTile2);

                if (x < 0 || x >= size || y < 0 || y >= size) continue;

                tilemap.SetTile(new Vector3Int(pos.x, pos.y, GroundZIndex), tiles.GrassTile);

                if (townLocations.Contains(new Vector2Int(x, y)))
                {
                    tilemap.SetTile(new Vector3Int(pos.x, pos.y, TownZIndex), tiles.TownTileT1);
                }
            }
        }

        onMapGenerated?.Invoke(size);
    }

    private void UpdateTown(Town town)
    {
        var tile = town.Tier switch
        {
            Tier.Tier1 => tiles.TownTileT1,
            Tier.Tier2 => tiles.TownTileT2,
            Tier.Tier3 => tiles.TownTileT3,
            _ => tiles.TownTileT3
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
            tilemap.SetTile(new Vector3Int(pos.x, pos.y, SelectionZIndex), tiles.SelectionTile);
            onTownClicked?.Invoke(town);
        }
        else
        {
            onGroundClicked?.Invoke();
        }
    }
}