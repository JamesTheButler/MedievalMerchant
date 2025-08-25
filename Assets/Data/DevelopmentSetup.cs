using System.Collections.Generic;
using UnityEngine;

namespace Data
{
    [CreateAssetMenu(fileName = "DevelopmentTables", menuName = "Data/DevelopmentTables")]
    public class DevelopmentTables : ScriptableObject
    {
        [SerializeField]
        private DevelopmentTable tier1Table;

        [SerializeField]
        private DevelopmentTable tier2Table;

        [SerializeField]
        private DevelopmentTable tier3Table;

        public IReadOnlyDictionary<Tier, DevelopmentTable> Tables => _tables ??= GenerateTableDictionary();

        private Dictionary<Tier, DevelopmentTable> _tables;

        private Dictionary<Tier, DevelopmentTable> GenerateTableDictionary()
        {
            return new Dictionary<Tier, DevelopmentTable>
            {
                { Tier.Tier1, tier1Table },
                { Tier.Tier2, tier2Table },
                { Tier.Tier3, tier3Table },
            };
        }
    }
}