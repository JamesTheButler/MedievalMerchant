using System.Collections.Generic;

namespace Unity.Localization.Roslyn;

internal sealed record SharedTableData(string Guid, Dictionary<long, string> Entries);