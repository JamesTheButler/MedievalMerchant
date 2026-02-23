using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine.Localization.Metadata;
using UnityEngine.Localization.Tables;

namespace Features.Localization
{
    public static class DummyExtensions
    {
        public static string GetId(this SharedTableData sharedTableData) => string.Empty;
        public static string[] GetEntryIds(this SmartFormatTag tag) => Array.Empty<string>();
        public static string GetSharedId(this StringTable stringTable) => string.Empty;
    }

    public sealed class SourceGenDummyImplementation
    {
        private void ExtractTableData()
        {
            var sharedDataTables = Array.Empty<SharedTableData>().ToDictionary(data => data.GetEntityId().ToString());
            var tables = Array.Empty<StringTable>().ToDictionary(data => data.SharedData.GetEntityId().ToString());

            var pairs = new Dictionary<SharedTableData, StringTable>();
            foreach (var (key, sharedTable) in sharedDataTables)
            {
                if (tables.TryGetValue(key, out var table))
                {
                    pairs.Add(sharedTable, table);
                }
            }

            foreach (var (sharedTable, table) in pairs)
            {
                foreach (var metaEntry in table.MetadataEntries)
                {
                    if (metaEntry is SmartFormatTag smartFormatTag)
                    {
                        foreach (var entry in smartFormatTag.GetEntryIds()) { }
                    }
                }
            }
        }
    }

    public sealed record TableInfo(Entry[] Entries);

    public sealed record Entry(
        long Id,
        string Key,
        string EnglishText,
        EntryArg[] Args);

    public sealed record EntryArg(string Name, string Type);

    // Used to mark an entry in a StringTable for source generation. Superfluous if the entry is marked as a smart string.
}