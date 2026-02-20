using System.Collections.Generic;
using System.Collections.Immutable;
using System.Text;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Text;

namespace MedievalMerchant.Generators
{
    [Generator]
    public sealed class LocalizationKeyGenerator : IIncrementalGenerator
    {
        private const string SharedDataSuffix = "Shared Data.asset";
        private const string EnLocaleSuffix = "_en.asset";

        public void Initialize(IncrementalGeneratorInitializationContext context)
        {
            // Collect all additional files
            var sharedDataFiles = context.AdditionalTextsProvider
                .Where(f => f.Path.EndsWith(SharedDataSuffix));

            var enLocaleFiles = context.AdditionalTextsProvider
                .Where(f => f.Path.EndsWith(EnLocaleSuffix));

            // Combine shared data and locale files
            var allFiles = sharedDataFiles.Collect().Combine(enLocaleFiles.Collect());

            context.RegisterSourceOutput(allFiles, (ctx, pair) =>
            {
                var sharedDatas = pair.Left;
                var enLocales = pair.Right;

                var tables = BuildTables(sharedDatas, enLocales, ctx);
                if (tables.Count == 0)
                    return;

                var source = LocClassEmitter.Emit(tables);
                ctx.AddSource("Loc.g.cs", SourceText.From(source, Encoding.UTF8));
            });
        }

        private static List<TableInfo> BuildTables(
            ImmutableArray<AdditionalText> sharedDatas,
            ImmutableArray<AdditionalText> enLocales,
            SourceProductionContext ctx)
        {
            var tables = new List<TableInfo>();

            // Parse all English locale files and index by table name
            var localeByTable = new Dictionary<string, Dictionary<long, string>>();
            foreach (var localeFile in enLocales)
            {
                var content = localeFile.GetText(ctx.CancellationToken)?.ToString();
                if (string.IsNullOrEmpty(content))
                    continue;

                var tableName = ExtractTableNameFromLocalePath(localeFile.Path);
                if (string.IsNullOrEmpty(tableName))
                    continue;

                var entries = LocaleParser.Parse(content);
                localeByTable[tableName] = entries;
            }

            // Parse all shared data files
            foreach (var sharedDataFile in sharedDatas)
            {
                var content = sharedDataFile.GetText(ctx.CancellationToken)?.ToString();
                if (string.IsNullOrEmpty(content))
                    continue;

                var table = SharedDataParser.Parse(content);
                if (string.IsNullOrEmpty(table.TableCollectionName) || table.Entries.Count == 0)
                    continue;

                // Merge English text from locale file
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

            return tables;
        }

        /// <summary>
        /// Extracts the table name from an English locale file path.
        /// e.g., "...Tables/Trade_en.asset" → "Trade"
        /// </summary>
        private static string ExtractTableNameFromLocalePath(string path)
        {
            // Normalize separators
            var normalized = path.Replace('\\', '/');
            var fileName = normalized.Substring(normalized.LastIndexOf('/') + 1);

            // Remove "_en.asset" suffix
            if (fileName.EndsWith(EnLocaleSuffix))
            {
                return fileName.Substring(0, fileName.Length - EnLocaleSuffix.Length);
            }

            return null;
        }
    }
}
