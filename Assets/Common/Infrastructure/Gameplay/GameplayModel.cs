using System.Collections.Generic;
using System.Linq;
using Common.Types;
using Features.Goods;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Model;
using Features.Levels.GameModifiers.Events;
using Features.Map;
using Features.Map.Tiling;
using Features.Player.Logic;
using Features.Stats;
using Features.Ticking.Logic;
using Features.Towns;
using UnityEngine;

namespace Common.Infrastructure.Gameplay
{
    public sealed class GameplayModel
    {
        public StatsModel Stats { get; set; } = new();
        public TileFlagMap TileFlagMap { get; private set; }
        public PlayerModel Player { get; private set; }

        public GameSpeedModel GameSpeed { get; } = new();
        public Date Date { get; } = new();
        public LevelConditions Conditions { get; } = new();
        public EventModel Events { get; } = new();
        public GoodPool GoodPool { get; private set; }

        public IReadOnlyDictionary<Vector2Int, Town> Towns => _towns;

        private Dictionary<Vector2Int, Town> _towns = new();

        public void Initialize(
            PlayerModel player,
            IEnumerable<Town> towns,
            TileFlagMap tileFlagMap,
            ConditionData[] conditions,
            ProductionZone[] productionZones)
        {
            _towns = towns.ToDictionary(town => town.GridLocation, town => town);
            Player = player;
            TileFlagMap = tileFlagMap;
            GoodPool = new GoodPool(productionZones);
            Conditions.Initialize(conditions);
        }
    }
}