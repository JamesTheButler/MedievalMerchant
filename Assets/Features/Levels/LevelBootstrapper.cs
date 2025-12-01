using Common;
using Features.Levels.Config;
using Features.Levels.Logic;
using Features.Map;
using Features.Map.Tiling;
using Features.Player;
using Infrastructure;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;

namespace Features.Levels
{
    public sealed class LevelBootstrapper : MonoBehaviour
    {
        [SerializeField]
        private UnityEvent completed;

        [SerializeField, Required]
        private Grid tileGrid;

        [SerializeField, Required]
        private LevelInfo debugLevelInfo;

        [SerializeField, Required]
        private ProductionZoneInteractions productionZoneInteractions;

        [SerializeField, Required]
        private LevelConditionManager levelConditionManager;

        private void Start()
        {
            var levelInfo = GlobalContext.CurrentLevelInfo ?? debugLevelInfo;
            var level = Instantiate(levelInfo.MapPrefab, tileGrid.gameObject.transform);
            var tilemap = level.GetComponent<Tilemap>();
            var flagMap = TilemapScanner.Scan(tilemap);
            var townPositions = flagMap.GetAllCells(TileType.Town);
            var zones = level.GetComponentsInChildren<ProductionZone>();
            productionZoneInteractions.Initialize(zones);

            var townFactory = new TownFactory();
            var towns = townFactory.GenerateTowns(townPositions, zones, tileGrid);
            var player = new PlayerModel(levelInfo.StartPlayerFunds);

            GameplayContext.Model.Initialize(player, towns, flagMap);
            GameplayContext.Systems.Initialize();

            var startTown = towns.GetRandom();
            player.Location.CurrentTown = startTown;
            player.Location.WorldLocation.Value = startTown.WorldLocation;
            player.CaravanManager.UpgradeCart(0);

            completed.Invoke();
        }

        public void CleanUp()
        {
            GameplayContext.Systems.CleanUp();
        }
    }
}