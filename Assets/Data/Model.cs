using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    public class Model
    {
        public Player Player { get; private set; }
        public IReadOnlyDictionary<Vector2Int, Town> Towns => _towns;

        private readonly Dictionary<Vector2Int, Town> _towns;

        public Model(IEnumerable<Town> towns)
        {
            Player = new Player();
            _towns = towns.ToDictionary(town => town.Location, town => town);
        }
    }
}