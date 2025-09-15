using System.Collections.Generic;
using System.Linq;
using Common;
using Data;
using Data.Configuration;
using Data.Towns;
using Map;
using Map.Tiling;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

public sealed class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private UnityEvent levelLoaded;

    [SerializeField, Required]
    private Grid tileGrid;

    [SerializeField, Required]
    private LevelInfo levelInfo;

    [SerializeField]
    private List<TownSetupInfo> townInfos;

    private const float ZoneDistance = 1.5f; // account for diagonally adjacent zones (distance here would be sqrt(2))

    private Model _model;

    private void Start()
    {
        _model = Model.Instance;

        var level = Instantiate(levelInfo.MapPrefab, tileGrid.gameObject.transform);
        var tilemap = level.GetComponent<Tilemap>();
        var flagMap = TilemapScanner.Scan(tilemap);
        var townPositions = flagMap.GetAllCells(TileType.Town);
        var zones = level.GetComponentsInChildren<ProductionZone>();
        var availableGoodsPerTown = FindAvailableGoodsPerTown(zones, townPositions);

        var towns = townPositions
            .Select(pos => new Town(
                townInfos.GetRandom(),
                pos,
                tileGrid.CellToWorld(pos.FromXY()),
                availableGoodsPerTown[pos]))
            .ToList();
        var player = new Player(levelInfo.StartPlayerFunds);

        _model.Initialize(player, towns, flagMap);

        var startTown = towns.GetRandom();
        player.Location.CurrentTown = startTown;
        player.Location.WorldLocation.Value = startTown.WorldLocation;

        levelLoaded.Invoke();
    }

    private static Dictionary<Vector2Int, HashSet<Good>> FindAvailableGoodsPerTown(ProductionZone[] zones,
        List<Vector2Int> townPositions)
    {
        var availableGoodsPerTown = new Dictionary<Vector2Int, HashSet<Good>>();
        foreach (var townPosition in townPositions)
        {
            availableGoodsPerTown[townPosition] = new HashSet<Good>();
        }

        foreach (var zone in zones)
        {
            foreach (var townPosition in townPositions)
            {
                if (zone.IsAdjacentTo(townPosition, ZoneDistance))
                {
                    availableGoodsPerTown[townPosition].AddRange(zone.AvailableGoods);
                }
            }
        }

        return availableGoodsPerTown;
    }
}