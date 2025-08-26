using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public sealed class Model : MonoBehaviour
    {
        public static Model Instance;

        public Player Player { get; private set; } = new(50000);
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

            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }
}