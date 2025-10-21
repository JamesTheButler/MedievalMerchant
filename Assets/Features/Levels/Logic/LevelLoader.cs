using System.Collections.Generic;
using System.Linq;
using Common;
using Common.Types;
using Features.Levels.Config;
using Features.Map;
using Features.Map.Tiling;
using Features.Player;
using Features.Towns;
using Features.Towns.Flags.Logic;
using NaughtyAttributes;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Serialization;
using UnityEngine.Tilemaps;

namespace Features.Levels.Logic
{
    public sealed class LevelLoader : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent levelLoaded;

        [SerializeField, Required]
        private Grid tileGrid;

        [SerializeField, Required]
        private LevelInfo debugLevelInfo;

        // 1.5 accounts for diagonally adjacent zones (where distance would be sqrt(2) == 1.41)
        private const float ZoneDistance = 1.5f;

        private Model _model;
        private FlagFactory _flagFactory;

        private LevelInfo _levelInfo;

        private void Start()
        {
            _model = Model.Instance;
            _flagFactory = new FlagFactory();

            FindLevelInfo();

            if (_levelInfo == null)
            {
                Debug.LogError($"Failed to load level. Neither {nameof(LevelLoadContext)} not debug level info found.");
                return;
            }

            var level = Instantiate(_levelInfo.MapPrefab, tileGrid.gameObject.transform);
            var tilemap = level.GetComponent<Tilemap>();
            var flagMap = TilemapScanner.Scan(tilemap);
            var townPositions = flagMap.GetAllCells(TileType.Town);
            var zones = level.GetComponentsInChildren<ProductionZone>();
            var towns = GenerateTowns(townPositions, zones);
            var player = new PlayerModel(_levelInfo.StartPlayerFunds);

            _model.Initialize(player, towns, flagMap);

            var startTown = towns.GetRandom();
            player.Location.CurrentTown = startTown;
            player.Location.WorldLocation.Value = startTown.WorldLocation;
            player.CaravanManager.UpgradeCart(0, 1);

            _model.ConditionManager.Setup(_levelInfo.Conditions);

            levelLoaded.Invoke();

            // clea level load context
            if (LevelLoadContext.Instance)
            {
                LevelLoadContext.Instance.SelectedLevel = null;
            }
        }

        private void FindLevelInfo()
        {
            if (LevelLoadContext.Instance?.SelectedLevel != null)
            {
                Debug.Log($"Loading Level Info from {nameof(LevelLoadContext)}");
                _levelInfo = LevelLoadContext.Instance.SelectedLevel;
            }
            else
            {
                Debug.Log("Loading Debug Level Info");
                _levelInfo = debugLevelInfo;
            }
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
            var worldPosition = tileGrid.CellToWorld(townPosition.FromXY());
            var townRegions = adjacentZones.Select(zone => zone.Regions).AggregateFlags();
            var availableGoods = GetAllZoneGoods(adjacentZones);

            var town = new Town(
                townPosition,
                worldPosition,
                townRegions,
                availableGoods,
                _flagFactory);
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
}