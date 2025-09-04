using System.Collections.Generic;
using System.Linq;
using Data.Towns;
using UnityEngine;

namespace Data
{
    public sealed class Model : MonoBehaviour
    {
        [SerializeField]
        private int playerStartFunds = 1000;

        public static Model Instance;

        public Date Date { get; private set; } = new();
        public Player Player { get; private set; }
        public IReadOnlyDictionary<Vector2Int, Town> Towns => _towns;

        private Dictionary<Vector2Int, Town> _towns = new();

        public void SetTowns(IEnumerable<Town> towns)
        {
            _towns = towns.ToDictionary(town => town.Location, town => town);
        }

        private void Awake()
        {
            if (Instance != null && Instance != this)
            {
                Destroy(gameObject);
                return;
            }

            Player = new Player(playerStartFunds);

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}