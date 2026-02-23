using System.Collections.Generic;

namespace MedievalMerchant.Generators
{
    public sealed class TableInfo
    {
        public string TableCollectionName { get; set; }
        public List<EntryInfo> Entries { get; set; } = new List<EntryInfo>();
    }
}
