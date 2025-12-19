using System.Collections.Generic;
using System.Linq;
using Common.Types;
using Features.Levels.Conditions.Data;
using Features.Levels.Conditions.Model;
using Features.Map.Tiling;
using Features.Player;
using Features.Towns;
using UnityEngine;

namespace Common.Infrastructure
{
    public sealed class GameplayModel
    {
        public TileFlagMap TileFlagMap { get; private set; }
        public PlayerModel Player { get; private set; }

        public Date Date { get; } = new();
        public LevelConditions Conditions { get; } = new();

        public IReadOnlyDictionary<Vector2Int, Town> Towns => _towns;

        private Dictionary<Vector2Int, Town> _towns = new();

        public void Initialize(
            PlayerModel player,
            IEnumerable<Town> towns,
            TileFlagMap tileFlagMap,
            ConditionData[] conditions)
        {
            _towns = towns.ToDictionary(town => town.GridLocation, town => town);
            Player = player;
            TileFlagMap = tileFlagMap;
            Conditions.Initialize(conditions);
        }
    }
}