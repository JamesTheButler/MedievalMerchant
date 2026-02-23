using System;
using System.Collections.Generic;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Diagnostics;

namespace Unity.Localization.Roslyn;

/// <summary>
/// Parses Unity Localization .asset files (Unity YAML) into <see cref="TableInfo"/> without
/// depending on Unity's runtime types.
///
/// Expects two files per table:
///   - SharedData asset  (*_Shared_Data.asset)   — provides ID → Key mapping
///   - Language asset    (*_en.asset, etc.)       — provides localized text, smart-string flags, args
/// </summary>
internal static class LocalizationAssetParser
{
    public static SharedTableData ParseSharedData(AdditionalText assetText, CancellationToken cancellationToken)
    {
        var guid = string.Empty;
        var entries = new Dictionary<long, string>();

        long currentId = -1;
        var inEntries = false;
        var entriesIndent = -1;
/*
        foreach (var rawLine in assetText.GetText(cancellationToken).Lines)
        {
            var line = rawLine.ToString().TrimStart();
            var indent = rawLine.ToString().Length - line.Length;

            // Top-level GUID
            if (TryGetValue(line, "m_TableCollectionNameGuidString", out var g))
            {
                guid = g;
                continue;
            }

            // Detect the m_Entries list — record indent so we know when it ends
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
            if (TryGetValue(stripped, "m_Id", out var idStr) && long.TryParse(idStr, out var id))
            {
                currentId = id;
                continue;
            }

            if (currentId >= 0 && TryGetValue(line, "m_Key", out var key))
            {
                entries[currentId] = key;
                currentId = -1;
            }
        }*/

        return new SharedTableData(guid, entries);
    }

    /// <summary>
    /// Parse a language asset file into a <see cref="TableInfo"/>.
    /// </summary>
    /// <param name="assetText">Full text of the language .asset file.</param>
    /// <param name="sharedData">Parsed shared data (for GUID validation and key lookup).</param>
    /// <param name="originalText">The <see cref="Microsoft.CodeAnalysis.AdditionalText"/> wrapper (may be null in tests).</param>
    /// <param name="className">Override for the generated class name; defaults to table collection name.</param>
    public static TableInfo Parse(
        string assetText,
        SharedTableData sharedData,
        AdditionalText? originalText = null,
        string? className = null)
    {
        // ---- Step 1: validate GUID cross-reference -------------------------
        string? langSharedGuid = null;
      /*  foreach (var line in EnumerateLines(assetText))
        {
            var m = Regex.Match(line, @"m_SharedData:.*guid:\s*([a-f0-9]+)");
            if (m.Success)
            {
                langSharedGuid = m.Groups[1].Value;
                break;
            }
        }

        if (langSharedGuid is null)
            throw new InvalidDataException("Language asset does not contain m_SharedData reference.");

        if (!string.Equals(langSharedGuid, sharedData.Guid, StringComparison.OrdinalIgnoreCase))
            throw new InvalidDataException(
                $"GUID mismatch: SharedData has '{sharedData.Guid}', language asset references '{langSharedGuid}'.");

        // ---- Step 2: collect all RIDs that are SmartFormatTag --------------
        var smartRids = new HashSet<string>(); // rid values of SmartFormatTag entries
        var smartIds = new HashSet<long>(); // entry IDs explicitly listed under SmartFormatTag

        // We do a two-pass parse of the references block to first find SmartFormatTag rids,
        // then collect the IDs stored inside them.
        CollectSmartInfo(assetText, smartRids, smartIds);

        // ---- Step 3: parse m_TableData -------------------------------------
        var tableEntries = ParseTableData(assetText, sharedData, smartIds);

        // Derive class name from the first key's table-name prefix, or from SharedData
        var resolvedClassName = className
                                ?? DeriveClassName(sharedData)
                                ?? "LocalizationTable";
*/
        return new TableInfo(originalText!, "");
    }

    // -----------------------------------------------------------------------
    //  Shared data helpers
    // -----------------------------------------------------------------------

    private static void CollectSmartInfo(string assetText, HashSet<string> smartRids, HashSet<long> smartIds)
    {
        // We need to correlate:
        //   - rid: <X>
        //     type: {class: SmartFormatTag ...}
        //     data:
        //       m_Entries:
        //       - id: <Y>
        //       m_SharedEntries:
        //       - id: <Z>
        //
        // Strategy: track the last seen rid; when we see SmartFormatTag on the type line, record the rid.
        // Then collect all id: lines until the next rid: or end of references block.

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
                inSmartBlock = false; // reset until we confirm type
                continue;
            }

            if (line.Contains("class: SmartFormatTag") && lastRid is not null)
            {
                smartRids.Add(lastRid);
                inSmartBlock = true;
                continue;
            }

            if (inSmartBlock && TryGetValue(line.TrimStart('-', ' '), "id", out var idStr)
                             && long.TryParse(idStr, out var id))
            {
                smartIds.Add(id);
            }
        }
    }

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
                // flush last
                if (currentId >= 0 && currentLocalized is not null)
                    results.Add(BuildEntry(currentId, currentLocalized, sharedData, smartIds));

                break;
            }

            if (!inTableData) continue;

            // New entry starts with "- m_Id:" (the dash is part of the YAML list item)
            if (line.StartsWith("- m_Id:"))
            {
                // flush previous
                if (currentId >= 0 && currentLocalized is not null)
                    results.Add(BuildEntry(currentId, currentLocalized, sharedData, smartIds));

                var idPart = line.TrimStart('-', ' ');
                if (TryGetValue(idPart, "m_Id", out var idStr) && long.TryParse(idStr, out var id))
                {
                    currentId = id;
                    currentLocalized = null;
                }

                continue;
            }

            if (currentId >= 0 && TryGetValue(line, "m_Localized", out var localized))
            {
                // Strip surrounding single quotes Unity sometimes adds
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

        foreach (Match m in SmartPlaceholderRegex.Matches(text))
        {
            var raw = m.Groups["n"].Value;
            var formatter = m.Groups["fmt"].Success ? m.Groups["fmt"].Value : null;

            if (seen.ContainsKey(raw)) continue; // deduplicate repeated args

            var (argName, argType) = ParseArgNameAndType(raw);
            var arg = new EntryArg(argName, argType, formatter);
            seen[raw] = arg;
            ordered.Add(arg);
        }

        return ordered.ToArray();
    }

    private static EntryArg[] ExtractPositionalArgs(string text)
    {
        var maxIndex = -1;
        var formatters = new Dictionary<int, string?>();

        foreach (Match m in PositionalPlaceholderRegex.Matches(text))
        {
            var idx = int.Parse(m.Groups["idx"].Value);
            if (idx > maxIndex) maxIndex = idx;
            if (m.Groups["fmt"].Success && !formatters.ContainsKey(idx))
                formatters[idx] = m.Groups["fmt"].Value;
        }

        if (maxIndex < 0) return Array.Empty<EntryArg>();

        var args = new EntryArg[maxIndex + 1];
        for (var i = 0; i <= maxIndex; i++)
        {
            formatters.TryGetValue(i, out var fmt);
            // Positional args are untyped — default to object/string per convention
            args[i] = new EntryArg($"arg{i}", "object", fmt);
        }

        return args;
    }

    /// <summary>
    /// Converts a raw placeholder name like "_int_MyArg" into ("MyArg", "int").
    /// Supported prefixes: _int_, _string_, _float_, _double_, _bool_, _long_
    /// No prefix → ("Name", "string")
    /// </summary>
    private static (string name, string type) ParseArgNameAndType(string raw)
    {
        var span = raw.AsSpan();

        return ("", "");/*
        
        if (span.StartsWith("_".AsSpan(), StringComparison.Ordinal))
        {
            // find second underscore
            var second = raw.IndexOf('_', 1);
            if (second > 1)
            {
                var prefix = raw[1..second].ToLowerInvariant();
                var name = raw[(second + 1)..];

                var type = prefix switch
                {
                    "int" => "int",
                    "string" => "string",
                    "float" => "float",
                    "double" => "double",
                    "bool" => "bool",
                    "long" => "long",
                    _ => "string" // unknown prefix → string
                };

                return (name, type);
            }
        }

        // No recognised prefix — treat entire name as the arg name, type = string
        return (raw, "string");*/
    }

    // -----------------------------------------------------------------------
    //  Utilities
    // -----------------------------------------------------------------------

    private static string DeriveClassName(SharedTableData sharedData)
    {
        // Use the first key to derive the table prefix (everything before the first dot)
  /*      foreach (var kv in sharedData.Entries)
        {
            var key = kv.Value;
            var dot = key.IndexOf('.');
            return dot > 0 ? key[..dot] : key;
        }
*/
        return "LocalizationTable";
    }

    /// <summary>
    /// Attempt to extract a value from a Unity YAML line of the form "key: value".
    /// Handles quoted and unquoted values.
    /// </summary>
    private static bool TryGetValue(string line, string key, out string value)
    {
     /*   var prefix = key + ":";
        if (!line.StartsWith(prefix))
        {
            value = string.Empty;
            return false;
        }

        value = line[prefix.Length..].Trim();
       */
     value = string.Empty;

     return true;
    }

    private static IEnumerable<string> EnumerateLines(string text)
    {
        using var reader = new StringReader(text);
        while (reader.ReadLine() is { } line)
            yield return line;
    }
}