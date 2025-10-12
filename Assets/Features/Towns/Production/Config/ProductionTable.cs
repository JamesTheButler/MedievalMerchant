using System.Collections.Generic;
using Common;
using Common.Types;
using UnityEngine;

namespace Features.Towns.Production.Config
{
    /// <summary>
    /// Table to define which goods CAN be produced. 
    /// </summary>
    [CreateAssetMenu(
        fileName = nameof(ProductionTable),
        menuName = AssetMenu.ConfigDataFolder + nameof(ProductionTable))]
    public sealed class ProductionTable : ScriptableObject
    {
        [field: SerializeField]
        public List<Good> Tier1Goods { get; private set; }

        [field: SerializeField]
        public List<Good> Tier2Goods { get; private set; }

        [field: SerializeField]
        public List<Good> Tier3Goods { get; private set; }
    }
}