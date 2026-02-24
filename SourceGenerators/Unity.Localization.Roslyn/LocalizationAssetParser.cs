using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.CodeAnalysis;

namespace Unity.Localization.Roslyn;

/// <summary>
/// Parses Unity Localization .asset files (Unity YAML) into <see cref="TableInfo"/> without
/// depending on Unity's runtime types.
///
/// Expects two files per table:
///   - SharedData asset  (*SharedData.asset)     — provides ID -> Key mapping and table collection name/GUID
///   - Language asset    (*_en.asset, etc.)       — provides localized text, smart-string flags, args
/// </summary>
internal static class LocalizationAssetParser
{
    public static SharedTableData ParseSharedData(AdditionalText assetText, CancellationToken cancellationToken)
    {
        var guid = string.Empty;
        var tableCollectionName = string.Empty;
        var entries = new Dictionary<long, string>();

        long currentId = -1;
        var inEntries = false;
        var entriesIndent = -1;

        foreach (var rawLine in assetText.GetText(cancellationToken)!.Lines)
        {
            var line = rawLine.ToString().TrimStart();
            var indent = rawLine.ToString().Length - line.Length;

            if (TryGetValue(line, "m_TableCollectionNameGuidString", out var foundGuid))
            {
                guid = foundGuid;
                continue;
            }

            if (TryGetValue(line, "m_TableCollectionName", out var foundName))
            {
                tableCollectionName = foundName;
                continue;
            }

            if (line == "m_Entries:")
            {
                inEntries = true;
                entriesIndent = indent;
                continue;
            }

            // Any non-list-item at same/lower indent as m_Entries: closes the block
            if (inEntries && !line.StartsWith("- ") && indent <= entriesIndent)
                inEntries = false;

            if (!inEntries) continue;

            var stripped = line.TrimStart('-', ' ');
            if (TryGetValue(stripped, "m_Id", out var idStr) && long.TryParse(idStr, out var parsedId))
            {
                currentId = parsedId;
                continue;
            }

            if (currentId >= 0 && TryGetValue(line, "m_Key", out var key))
            {
                entries[currentId] = key;
                currentId = -1;
            }
        }

        return new SharedTableData(guid, tableCollectionName, entries);
    }

    /// <summary>
    /// Parse a language asset file into a <see cref="TableInfo"/>.
    /// </summary>
    public static TableInfo Parse(
        string assetText,
        SharedTableData sharedData,
        string? className = null)
    {
        // Step 1: validate GUID cross-reference
        string? langSharedGuid = null;
        foreach (var line in EnumerateLines(assetText))
        {
            var match = Regex.Match(line, @"m_SharedData:.*guid:\s*([a-f0-9]+)");
            if (match.Success)
            {
                langSharedGuid = match.Groups[1].Value;
                break;
            }
        }

        if (langSharedGuid is null)
            throw new InvalidDataException("Language asset does not contain m_SharedData reference.");

        if (!string.Equals(langSharedGuid, sharedData.Guid, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException(
                $"GUID mismatch: SharedData has '{sharedData.Guid}', language asset references '{langSharedGuid}'.");

        // Step 2: collect all entry IDs that are smart strings
        var smartRids = new HashSet<string>();
        var smartIds = new HashSet<long>();
        CollectSmartInfo(assetText, smartRids, smartIds);

        // Step 3: parse m_TableData
        var tableEntries = ParseTableData(assetText, sharedData, smartIds);

        // Derive class name from table collection name
        var resolvedClassName = className
                                ?? DeriveClassName(sharedData)
                                ?? "LocalizationTable";

        return new TableInfo(sharedData.TableCollectionName, resolvedClassName, tableEntries);
    }

    // -----------------------------------------------------------------------
    //  Smart string collection
    // -----------------------------------------------------------------------

    private static void CollectSmartInfo(string assetText, HashSet<string> smartRids, HashSet<long> smartIds)
    {
        string? lastRid = null;
        var inSmartBlock = false;
        var inRefIds = false;

        foreach (var rawLine in EnumerateLines(assetText))
        {
            var line = rawLine.TrimStart();

            if (line == "RefIds:")
            {
                inRefIds = true;
                continue;
            }

            if (!inRefIds) continue;

            // New rid entry
            if (TryGetValue(line.TrimStart('-', ' '), "rid", out var rid))
            {
                lastRid = rid;
                inSmartBlock = false;
                continue;
            }

            if (line.Contains("class: SmartFormatTag") && lastRid is not null)
            {
                smartRids.Add(lastRid);
                inSmartBlock = true;
                continue;
            }

            if (inSmartBlock && TryGetValue(line.TrimStart('-', ' '), "id", out var idStr)
                             && long.TryParse(idStr, out var entryId))
            {
                smartIds.Add(entryId);
            }
        }
    }

    // -----------------------------------------------------------------------
    //  Table data parsing
    // -----------------------------------------------------------------------

    private static EntryInfo[] ParseTableData(
        string assetText,
        SharedTableData sharedData,
        HashSet<long> smartIds)
    {
        var results = new List<EntryInfo>();

        long currentId = -1;
        string? currentLocalized = null;
        var inTableData = false;

        foreach (var rawLine in EnumerateLines(assetText))
        {
            var line = rawLine.TrimStart();
            var indent = rawLine.Length - line.Length;

            if (line == "m_TableData:")
            {
                inTableData = true;
                continue;
            }

            // references: at indent 2 ends table data
            if (inTableData && indent <= 2 && line.StartsWith("references:"))
            {
                if (currentId >= 0 && currentLocalized is not null)
                    results.Add(BuildEntry(currentId, currentLocalized, sharedData, smartIds));

                break;
            }

            if (!inTableData) continue;

            if (line.StartsWith("- m_Id:"))
            {
                // flush previous
                if (currentId >= 0 && currentLocalized is not null)
                    results.Add(BuildEntry(currentId, currentLocalized, sharedData, smartIds));

                var idPart = line.TrimStart('-', ' ');
                if (TryGetValue(idPart, "m_Id", out var idStr) && long.TryParse(idStr, out var parsedId))
                {
                    currentId = parsedId;
                    currentLocalized = null;
                }

                continue;
            }

            if (currentId >= 0 && TryGetValue(line, "m_Localized", out var localized))
            {
                currentLocalized = localized.Trim('\'');
            }
        }

        // flush last if file ended without hitting references:
        if (currentId >= 0 && currentLocalized is not null)
            results.Add(BuildEntry(currentId, currentLocalized, sharedData, smartIds));

        return results.ToArray();
    }

    private static EntryInfo BuildEntry(
        long id,
        string localizedText,
        SharedTableData sharedData,
        HashSet<long> smartIds)
    {
        sharedData.Entries.TryGetValue(id, out var key);
        key ??= id.ToString();

        var isSmart = smartIds.Contains(id);

        var args = isSmart
            ? ExtractSmartArgs(localizedText)
            : ExtractPositionalArgs(localizedText);

        return new EntryInfo(id, key, localizedText, args);
    }

    // -----------------------------------------------------------------------
    //  Argument extraction
    // -----------------------------------------------------------------------

    // Matches smart placeholders: {Name}, {_int_Name}, {_string_Name:Formatter}
    // Excludes purely numeric positional args like {0}
    private static readonly Regex SmartPlaceholderRegex =
        new(@"\{(?<name>[A-Za-z_][A-Za-z0-9_]*)(?::(?<fmt>[^}]+))?\}", RegexOptions.Compiled);

    // Matches positional args: {0}, {1:D2}
    private static readonly Regex PositionalPlaceholderRegex =
        new(@"\{(?<idx>\d+)(?::(?<fmt>[^}]+))?\}", RegexOptions.Compiled);

    private static EntryArg[] ExtractSmartArgs(string text)
    {
        var seen = new Dictionary<string, EntryArg>(StringComparer.Ordinal);
        var ordered = new List<EntryArg>();

        foreach (Match match in SmartPlaceholderRegex.Matches(text))
        {
            var raw = match.Groups["name"].Value;
            var formatter = match.Groups["fmt"].Success ? match.Groups["fmt"].Value : null;

            if (seen.ContainsKey(raw)) continue;

            var (argName, argType) = ParseArgNameAndType(raw);
            var arg = new EntryArg(argName, argType, raw, formatter);
            seen[raw] = arg;
            ordered.Add(arg);
        }

        return ordered.ToArray();
    }

    private static EntryArg[] ExtractPositionalArgs(string text)
    {
        var maxIndex = -1;
        var formatters = new Dictionary<int, string?>();

        foreach (Match match in PositionalPlaceholderRegex.Matches(text))
        {
            var idx = int.Parse(match.Groups["idx"].Value);
            if (idx > maxIndex) maxIndex = idx;
            if (match.Groups["fmt"].Success && !formatters.ContainsKey(idx))
                formatters[idx] = match.Groups["fmt"].Value;
        }

        if (maxIndex < 0) return Array.Empty<EntryArg>();

        var args = new EntryArg[maxIndex + 1];
        for (var index = 0; index <= maxIndex; index++)
        {
            formatters.TryGetValue(index, out var fmt);
            args[index] = new EntryArg($"arg{index}", "object", fmt);
        }

        return args;
    }

    /// <summary>
    /// Converts a raw placeholder name like "_int_MyArg" into ("MyArg", "int").
    /// Supported prefixes: _int_, _string_, _float_, _double_, _bool_, _long_
    /// No prefix -> ("Name", "string")
    /// </summary>
    private static (string name, string type) ParseArgNameAndType(string raw)
    {
        if (raw.Length > 1 && raw[0] == '_')
        {
            var second = raw.IndexOf('_', 1);
            if (second > 1)
            {
                var prefix = raw.Substring(1, second - 1).ToLowerInvariant();
                var name = raw.Substring(second + 1);

                var type = prefix switch
                {
                    "int" => "int",
                    "string" => "string",
                    "float" => "float",
                    "double" => "double",
                    "bool" => "bool",
                    "long" => "long",
                    _ => "string"
                };

                return (name, type);
            }
        }

        return (raw, "string");
    }

    // -----------------------------------------------------------------------
    //  Utilities
    // -----------------------------------------------------------------------

    private static string DeriveClassName(SharedTableData sharedData)
    {
        if (!string.IsNullOrEmpty(sharedData.TableCollectionName))
            return sharedData.TableCollectionName;

        // Fallback: use the first key to derive the table prefix
        foreach (var keyValuePair in sharedData.Entries)
        {
            var key = keyValuePair.Value;
            var dot = key.IndexOf('.');
            return dot > 0 ? key.Substring(0, dot) : key;
        }

        return "LocalizationTable";
    }

    /// <summary>
    /// Attempt to extract a value from a Unity YAML line of the form "key: value".
    /// Handles quoted and unquoted values.
    /// </summary>
    private static bool TryGetValue(string line, string key, out string value)
    {
        var prefix = key + ":";
        if (!line.StartsWith(prefix))
        {
            value = string.Empty;
            return false;
        }

        value = line.Substring(prefix.Length).Trim();
        return true;
    }

    private static IEnumerable<string> EnumerateLines(string text)
    {
        using var reader = new StringReader(text);
        while (reader.ReadLine() is { } line)
            yield return line;
    }
}
