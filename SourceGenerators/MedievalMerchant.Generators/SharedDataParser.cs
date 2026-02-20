using System.Collections.Generic;

namespace MedievalMerchant.Generators
{
    /// <summary>
    /// Parses a Unity Localization "*Shared Data.asset" YAML file to extract
    /// the table collection name and all entry keys with their IDs.
    /// </summary>
    public static class SharedDataParser
    {
        public static TableInfo Parse(string content)
        {
            var table = new TableInfo();
            var lines = content.Split('\n');

            long currentId = 0;
            bool inEntries = false;

            for (int i = 0; i < lines.Length; i++)
            {
                var line = lines[i].TrimEnd('\r');
                var trimmed = line.TrimStart();

                // Extract table collection name
                if (trimmed.StartsWith("m_TableCollectionName:"))
                {
                    table.TableCollectionName = ExtractValue(trimmed);
                    continue;
                }

                // Detect start of entries array
                if (trimmed.StartsWith("m_Entries:"))
                {
                    inEntries = true;
                    continue;
                }

                if (!inEntries)
                    continue;

                // Detect end of entries (next top-level field)
                if (!line.StartsWith("  ") && !line.StartsWith("\t") && trimmed.Length > 0 && !trimmed.StartsWith("-"))
                {
                    // We've left the m_Entries block
                    if (trimmed.StartsWith("m_"))
                        break;
                }

                // Strip YAML list item marker "- " prefix
                var field = trimmed;
                if (field.StartsWith("- "))
                    field = field.Substring(2);

                // Parse entry ID
                if (field.StartsWith("m_Id:"))
                {
                    var idStr = ExtractValue(field);
                    if (long.TryParse(idStr, out var id))
                        currentId = id;
                    continue;
                }

                // Parse entry key
                if (field.StartsWith("m_Key:"))
                {
                    var key = ExtractValue(field);
                    if (!string.IsNullOrEmpty(key))
                    {
                        table.Entries.Add(new EntryInfo
                        {
                            Id = currentId,
                            Key = key
                        });
                    }
                    continue;
                }
            }

            return table;
        }

        private static string ExtractValue(string line)
        {
            var colonIdx = line.IndexOf(':');
            if (colonIdx < 0 || colonIdx + 1 >= line.Length)
                return "";
            return line.Substring(colonIdx + 1).Trim();
        }
    }
}
