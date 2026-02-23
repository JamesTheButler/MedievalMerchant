using System;
using System.Collections.Generic;
using System.IO;
using MedievalMerchant.Generators;

// Quick smoke test: parse real asset files and emit the Loc class

var tablesDir = Path.GetFullPath(Path.Combine(
    AppContext.BaseDirectory, "..", "..", "..", "..", "..",
    "Assets", "Features", "Localization", "Data", "Tables"));

Console.WriteLine($"Tables dir: {tablesDir}");
Console.WriteLine($"Exists: {Directory.Exists(tablesDir)}");
Console.WriteLine();

var sharedDataFiles = Directory.GetFiles(tablesDir, "*Shared Data.asset");
var enLocaleFiles = Directory.GetFiles(tablesDir, "*_en.asset");

Console.WriteLine($"Found {sharedDataFiles.Length} Shared Data files");
Console.WriteLine($"Found {enLocaleFiles.Length} English locale files");
Console.WriteLine();

// Parse locale files
var localeByTable = new Dictionary<string, Dictionary<long, string>>();
foreach (var f in enLocaleFiles)
{
    var fileName = Path.GetFileName(f);
    var tableName = fileName.Replace("_en.asset", "");
    var entries = LocaleParser.Parse(File.ReadAllText(f));
    localeByTable[tableName] = entries;
    Console.WriteLine($"  Locale: {tableName} -> {entries.Count} entries");
}
Console.WriteLine();

// Parse shared data and merge
var tables = new List<TableInfo>();
foreach (var f in sharedDataFiles)
{
    var table = SharedDataParser.Parse(File.ReadAllText(f));
    Console.WriteLine($"  Table: {table.TableCollectionName} -> {table.Entries.Count} entries");

    if (localeByTable.TryGetValue(table.TableCollectionName, out var localeEntries))
    {
        foreach (var entry in table.Entries)
        {
            if (localeEntries.TryGetValue(entry.Id, out var englishText))
            {
                entry.EnglishText = englishText;
                entry.ArgCount = LocaleParser.CountArguments(englishText);
            }
        }
    }

    tables.Add(table);
}

Console.WriteLine();
Console.WriteLine("=== Generated Code (first 100 lines) ===");
Console.WriteLine();

var source = LocClassEmitter.Emit(tables);
var lines = source.Split('\n');
for (int i = 0; i < Math.Min(100, lines.Length); i++)
{
    Console.WriteLine(lines[i]);
}

Console.WriteLine();
Console.WriteLine($"Total lines: {lines.Length}");

// Write full output to file for inspection
var outputPath = Path.Combine(AppContext.BaseDirectory, "Loc.g.cs");
File.WriteAllText(outputPath, source);
Console.WriteLine($"Full output written to: {outputPath}");
