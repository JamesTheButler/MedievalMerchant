using System.Collections.Generic;
using System.Linq;

namespace Data
{
    public class Model
    {
        public Player Player { get; private set; }
        
        public IReadOnlyList<Town> Towns => _towns;
        private readonly List<Town> _towns;

        public Model(IEnumerable<Town> towns)
        {
            Player = new Player();
            _towns = towns.ToList();
        }
    }
}