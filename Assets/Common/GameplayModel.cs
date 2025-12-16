using System.Collections.Generic;
using System.Linq;
using Common.Types;
using Features.Levels.Conditions.Model;
using Features.Map.Tiling;
using Features.Player;
using Features.Towns;
using UnityEngine;

namespace Common
{
    public sealed class GameplayModel
    {
        public TileFlagMap TileFlagMap { get; private set; }
        public Date Date { get; private set; } = new();
        public PlayerModel Player { get; private set; }
        public LevelModifiers Modifiers { get; private set; } = new();
        public LevelConditions Conditions { get; private set; } = new();

        public IReadOnlyDictionary<Vector2Int, Town> Towns => _towns;

        private Dictionary<Vector2Int, Town> _towns = new();

        public void Initialize(
            PlayerModel player,
            IEnumerable<Town> towns,
            TileFlagMap tileFlagMap)
        {
            _towns = towns.ToDictionary(town => town.GridLocation, town => town);
            Player = player;
            TileFlagMap = tileFlagMap;
        }
    }
}