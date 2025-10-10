using System.Collections.Generic;
using System.Linq;
using Common;
using Data;
using Data.Configuration;
using Data.Player;
using Data.Towns;
using Levels;
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
        var towns = GenerateTowns(townPositions, zones);
        var player = new PlayerModel(levelInfo.StartPlayerFunds);

        _model.Initialize(player, towns, flagMap);

        var startTown = towns.GetRandom();
        player.Location.CurrentTown = startTown;
        player.Location.WorldLocation.Value = startTown.WorldLocation;
        player.CaravanManager.UpgradeCart(0, 1);

        _model.ConditionManager.Setup(levelInfo.Conditions);

        levelLoaded.Invoke();
    }

    private List<Town> GenerateTowns(List<Vector2Int> townPositions, ProductionZone[] zones)
    {
        var towns = new List<Town>();
        var zonesPerTown = GetZonesPerTown(townPositions, zones);

        foreach (var townPosition in townPositions)
        {
            var town = GenerateTown(townPosition, zonesPerTown[townPosition]);
            towns.Add(town);
        }

        return towns;
    }

    private Town GenerateTown(Vector2Int townPosition, List<ProductionZone> adjacentZones)
    {
        // BUG: setup should be derived from neighboring zones
        var setup = townInfos.GetRandom();
        var worldPosition = tileGrid.CellToWorld(townPosition.FromXY());
        var townRegions = adjacentZones.Select(zone => zone.Regions).AggregateFlags();
        var availableGoods = GetAllZoneGoods(adjacentZones);

        var town = new Town(
            setup,
            townPosition,
            worldPosition,
            townRegions,
            availableGoods);
        return town;
    }

    private static Dictionary<Vector2Int, List<ProductionZone>> GetZonesPerTown(
        List<Vector2Int> townPositions,
        ProductionZone[] zones)
    {
        var zonesPerTown = new Dictionary<Vector2Int, List<ProductionZone>>();

        foreach (var townPosition in townPositions)
        {
            zonesPerTown.Add(townPosition, new List<ProductionZone>());
            foreach (var zone in zones)
            {
                if (zone.IsAdjacentTo(townPosition, ZoneDistance))
                {
                    zonesPerTown[townPosition].Add(zone);
                }
            }
        }

        return zonesPerTown;
    }

    private static HashSet<Good> GetAllZoneGoods(List<ProductionZone> zones)
    {
        var allGoods = new HashSet<Good>();

        foreach (var zone in zones)
        {
            allGoods.AddRange(zone.AvailableGoods);
        }

        return allGoods;
    }
}