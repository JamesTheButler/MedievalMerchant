using Common.Infrastructure;
using Common.UI.Elements;
using Common.Utility;
using Features.Map;
using Features.Map.Tiling;
using Features.Player.Logic;
using Features.Towns;
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

            var context = GameplayContext.Instance;
            context.Model.Initialize(player, towns, flagMap, levelInfo.Conditions, zones);
            context.Services.Initialize();
            context.Systems.Initialize();

            var startTown = towns.GetRandom();
            player.Location.CurrentTown = startTown;
            player.Location.WorldLocation.Value = startTown.WorldLocation;

            player.CaravanManager.UpgradeCart(0);

            var modifierService = context.Services.GameModifierService;
            modifierService.ApplyModifier(levelInfo.GameplayModifiers);

            InitializeUI();

            completed.Invoke();
        }

        private void InitializeUI()
        {
            var initializables = Resources.FindObjectsOfTypeAll<InitializableUI>();
            foreach (var initializable in initializables)
            {
                initializable.Initialize();
            }
        }
    }
}