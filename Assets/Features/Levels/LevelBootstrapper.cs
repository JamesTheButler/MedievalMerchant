using System.Collections.Generic;
using System.Linq;
using Common.Infrastructure.Gameplay;
using Common.Infrastructure.Global;
using Common.UI.Elements;
using Common.Utility;
using Features.Audio.Music;
using Features.Levels.GameModifiers.Effects.Data;
using Features.Map.Tiling;
using Features.Map.Zones;
using Features.Player.Camp.Logic;
using Features.Player.Logic;
using Features.Towns;
using Features.Towns.Initialization;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.Tilemaps;
using Random = UnityEngine.Random;

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

        private void Start()
        {
            var levelInfo = GlobalContext.CurrentLevelInfo ?? debugLevelInfo;
            var level = Instantiate(levelInfo.MapPrefab, tileGrid.gameObject.transform);
            var tilemap = level.GetComponent<Tilemap>();
            var flagMap = TilemapScanner.Scan(tilemap);
            var townPositions = flagMap.GetAllCells(TileType.Town);
            var townInitializers = level
                .GetComponentsInChildren<TownInitializer>()
                .ToDictionary(initializer => initializer.GridPosition, initializer => initializer);

            var zones = level.GetComponentsInChildren<ProductionZone>();

            var townFactory = new TownFactory();
            var towns = townFactory.GenerateTowns(townPositions, townInitializers, zones, tileGrid, flagMap.TownTiles);

            var player = new PlayerModel(levelInfo.StartPlayerFunds);

            var context = GameplayContext.Instance;
            context.Model.Initialize(player, towns, flagMap, levelInfo.Conditions, zones);

            var campPositions = flagMap.GetAllCells(TileType.Camp);
            if (campPositions.Count > 0)
            {
                var campPos = campPositions[0];
                var campWorldPos = tileGrid.CellToWorld(campPos.FromXY()).XY();
                var campTile = flagMap.CampTiles[campPos];
                var camp = new Camp(campPos, campWorldPos, campTile);
                context.Model.SetCamp(camp);
            }

            if (campPositions.Count > 1)
            {
                Debug.LogError($"There are more than one camp in {levelInfo.LevelName.GetLocalizedString()}.");
            }

            context.Services.Initialize();
            context.Systems.Initialize();

            SetStartTown(levelInfo, towns, player);

            player.CaravanManager.UpgradeCart(0);

            var modifierService = context.Services.GameModifierService;
            modifierService.ApplyModifier(levelInfo.GameplayModifiers);

            InitializeEverything();

            GlobalContext.Instance.Services.MusicService.MusicModeChange.Invoke(MusicMode.Gameplay);

            completed.Invoke();
        }

        private static void SetStartTown(LevelInfo levelInfo, List<Town> towns, PlayerModel player)
        {
            var allyEffect = levelInfo.GameplayModifiers.Effects.FirstOfType<AllyEffectData, EffectData>();
            var levelInfoStartIndex = levelInfo.StartTownIndex;

            var possibleTowns = allyEffect != null
                ? towns.Where(town => town.MainRegion == allyEffect.AllyRegion).ToList()
                : towns;

            if (levelInfoStartIndex == -1)
            {
                levelInfoStartIndex = Random.Range(0, possibleTowns.Count);
            }

            var startTown = possibleTowns[levelInfoStartIndex];
            player.Location.MapLocation.Value = startTown;
            player.Location.WorldLocation.Value = startTown.WorldLocation;
        }

        private void OnDestroy()
        {
            CleanUpEverything();
        }

        private void InitializeEverything()
        {
            var initializables = Resources.FindObjectsOfTypeAll<InitializableBehavior>();
            foreach (var initializable in initializables)
            {
                initializable.Initialize();
            }
        }

        private void CleanUpEverything()
        {
            var initializables = Resources.FindObjectsOfTypeAll<InitializableBehavior>();
            foreach (var initializable in initializables)
            {
                initializable.CleanUp();
            }
        }
    }
}