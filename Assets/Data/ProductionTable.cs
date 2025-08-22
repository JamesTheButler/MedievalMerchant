using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Data
{
    /// <summary>
    /// Table to define which goods CAN be produced. 
    /// </summary>
    [CreateAssetMenu(fileName = "ProductionTable", menuName = "Data/ProductionTable")]
    public class ProductionTable : ScriptableObject
    {
        [field: SerializeField] public List<Good> Tier1Goods { get; private set; }
        [field: SerializeField] public List<Good> Tier2Goods { get; private set; }
        [field: SerializeField] public List<Good> Tier3Goods { get; private set; }
    }
}