using System.Collections.Generic;
using System.Linq;
using Common.Types;
using Features.Levels.Config;
using Features.Levels.Logic;
using Features.Map.Tiling;
using Features.Player;
using Features.Towns;
using UnityEngine;

namespace Common
{
    public sealed class GameplayModel : MonoBehaviour
    {
        public static GameplayModel Instance;

        public LevelInfo LevelInfo { get; private set; }
        public TileFlagMap TileFlagMap { get; private set; }
        public Date Date { get; private set; } = new();
        public PlayerModel Player { get; private set; }

        // TODO - STYLE: Model shouldn't hold systems and ConditionManager is a system (it's game logic)
        public ConditionManager ConditionManager { get; private set; }

        public IReadOnlyDictionary<Vector2Int, Town> Towns => _towns;

        private Dictionary<Vector2Int, Town> _towns = new();

        // TODO - STYLE: Model shouldn't hold systems
        private DividendsSystem _dividendsSystem;

        public void Initialize(
            PlayerModel player,
            IEnumerable<Town> towns,
            TileFlagMap tileFlagMap,
            LevelInfo levelInfo)
        {
            LevelInfo = levelInfo;
            _towns = towns.ToDictionary(town => town.GridLocation, town => town);
            Player = player;
            TileFlagMap = tileFlagMap;
            ConditionManager = gameObject.GetComponent<ConditionManager>();

            _dividendsSystem = new DividendsSystem();
            _dividendsSystem.Initialize();
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
        }
    }
}