using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "ProductionTable", menuName = "Data/ProductionTable")]
    public class ProductionTable : ScriptableObject
    {
        [field: SerializeField] public List<Good> Tier1Goods { get; private set; }
        [field: SerializeField] public List<Good> Tier2Goods { get; private set; }
        [field: SerializeField] public List<Good> Tier3Goods { get; private set; }

        public IEnumerable<Good> GetProduction()
        {
            return Tier1Goods.Concat(Tier2Goods).Concat(Tier3Goods);
        }
    }
}