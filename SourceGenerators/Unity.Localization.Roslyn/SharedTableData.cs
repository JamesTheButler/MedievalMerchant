using System.Collections.Generic;

namespace Unity.Localization.Roslyn;

internal sealed record SharedTableData(string Guid, string TableCollectionName, Dictionary<long, string> Entries);
