using System.Collections.Generic;
using System.Linq;
using Data.Towns;
using Tilemap;
using UnityEngine;

namespace Data
{
    public sealed class Model : MonoBehaviour
    {
        public static Model Instance;

        public TileFlagMap TileFlagMap { get; private set; }
        public Date Date { get; private set; } = new();
        public Player Player { get; private set; }
        public IReadOnlyDictionary<Vector2Int, Town> Towns => _towns;

        private Dictionary<Vector2Int, Town> _towns = new();

        public void Initialize(Player player, IEnumerable<Town> towns, TileFlagMap tileFlagMap)
        {
            _towns = towns.ToDictionary(town => town.GridLocation, town => town);
            Player = player;
            TileFlagMap = tileFlagMap;
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}