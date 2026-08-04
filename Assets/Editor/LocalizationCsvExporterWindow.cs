using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Metadata;

namespace Editor
{
    /// <summary>
    /// Batch-exports every (or a chosen subset of) String Table Collection to its own CSV file,
    /// restricted to a chosen subset of locales, instead of exporting each collection manually
    /// through the Localization Tables window.
    /// </summary>
    public sealed class LocalizationCsvExporterWindow : EditorWindow
    {
        // Not a real translation - a pseudo-locale holding a description of each entry for translators.
        // Always exported alongside every locale rather than offered as a selectable locale.
        private const string DescriptionLocaleCode = "comment";

        // The source-of-truth text. Always exported alongside every locale so translators always
        // have the original English to work from, rather than offered as a selectable locale.
        private const string EnglishLocaleCode = "en";

        private const string OutputFolderPrefKey = "MedievalMerchant.LocalizationCsvExporter.OutputFolder";
        private const string IncludeIdPrefKey = "MedievalMerchant.LocalizationCsvExporter.IncludeId";
        private const string IncludeCommentsPrefKey = "MedievalMerchant.LocalizationCsvExporter.IncludeComments";

        private List<Locale> _locales = new();
        private Locale _descriptionLocale;
        private Locale _englishLocale;
        private List<StringTableCollection> _collections = new();
        private readonly Dictionary<string, bool> _localeSelection = new();
        private readonly Dictionary<string, bool> _collectionSelection = new();

        private bool _includeId = true;
        private bool _includeComments;
        private string _outputFolder = "";
        private Vector2 _localeScroll;
        private Vector2 _collectionScroll;

        [MenuItem("Tools/Localization/CSV Exporter")]
        private static void Open()
        {
            var window = GetWindow<LocalizationCsvExporterWindow>("Localization CSV Exporter");
            window.minSize = new Vector2(420, 520);
        }

        private void OnEnable()
        {
            _outputFolder = EditorPrefs.GetString(OutputFolderPrefKey, "");
            _includeId = EditorPrefs.GetBool(IncludeIdPrefKey, true);
            _includeComments = EditorPrefs.GetBool(IncludeCommentsPrefKey, false);
            RefreshData();
        }

        private void RefreshData()
        {
            var allLocales = LocalizationEditorSettings.GetLocales();
            _descriptionLocale = allLocales.FirstOrDefault(l => l.Identifier.Code == DescriptionLocaleCode);
            _englishLocale = allLocales.FirstOrDefault(l => l.Identifier.Code == EnglishLocaleCode);
            _locales = allLocales.Where(l => l.Identifier.Code != DescriptionLocaleCode && l.Identifier.Code != EnglishLocaleCode)
                .OrderBy(l => l.Identifier.Code).ToList();
            _collections = LocalizationEditorSettings.GetStringTableCollections().OrderBy(c => c.TableCollectionName).ToList();

            foreach (var locale in _locales)
            {
                if (!_localeSelection.ContainsKey(locale.Identifier.Code))
                    _localeSelection[locale.Identifier.Code] = true;
            }

            foreach (var collection in _collections)
            {
                if (!_collectionSelection.ContainsKey(collection.TableCollectionName))
                    _collectionSelection[collection.TableCollectionName] = true;
            }
        }

        private void OnGUI()
        {
            if (GUILayout.Button("Refresh"))
                RefreshData();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Locales to export", EditorStyles.boldLabel);
            if (_descriptionLocale != null || _englishLocale != null)
                EditorGUILayout.HelpBox("English and the 'comment' locale are always included (as English + Description columns).", MessageType.None);
            DrawSelectAllNoneButtons(_locales.Select(l => l.Identifier.Code), _localeSelection);

            _localeScroll = EditorGUILayout.BeginScrollView(_localeScroll, GUILayout.Height(Mathf.Min(140, _locales.Count * 20 + 8)));
            foreach (var locale in _locales)
            {
                var code = locale.Identifier.Code;
                _localeSelection[code] = EditorGUILayout.ToggleLeft(locale.LocaleName, _localeSelection[code]);
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Table collections to export", EditorStyles.boldLabel);
            DrawSelectAllNoneButtons(_collections.Select(c => c.TableCollectionName), _collectionSelection);

            _collectionScroll = EditorGUILayout.BeginScrollView(_collectionScroll, GUILayout.Height(Mathf.Min(220, _collections.Count * 20 + 8)));
            foreach (var collection in _collections)
            {
                var name = collection.TableCollectionName;
                _collectionSelection[name] = EditorGUILayout.ToggleLeft(name, _collectionSelection[name]);
            }
            EditorGUILayout.EndScrollView();

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Options", EditorStyles.boldLabel);
            _includeId = EditorGUILayout.ToggleLeft(
                "Include Id column (recommended - lets you rename Keys and safely re-import)", _includeId);
            _includeComments = EditorGUILayout.ToggleLeft(
                "Include comment columns (Shared + per-locale)", _includeComments);

            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Output folder", EditorStyles.boldLabel);
            EditorGUILayout.BeginHorizontal();
            _outputFolder = EditorGUILayout.TextField(_outputFolder);
            if (GUILayout.Button("Browse...", GUILayout.Width(80)))
            {
                var picked = EditorUtility.SaveFolderPanel("Select export folder", _outputFolder, "");
                if (!string.IsNullOrEmpty(picked))
                    _outputFolder = picked;
            }
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space();

            var selectedLocales = _locales.Where(l => _localeSelection[l.Identifier.Code]).ToList();
            var selectedCollections = _collections.Where(c => _collectionSelection[c.TableCollectionName]).ToList();

            var columnCount = selectedLocales.Count + (_descriptionLocale != null ? 1 : 0) + (_englishLocale != null ? 1 : 0);
            using (new EditorGUI.DisabledScope(selectedLocales.Count == 0 || selectedCollections.Count == 0 || string.IsNullOrEmpty(_outputFolder)))
            {
                if (GUILayout.Button($"Export {selectedCollections.Count} CSV file(s), {columnCount} locale column(s) each", GUILayout.Height(32)))
                    Export(selectedLocales, selectedCollections);
            }
        }

        private static void DrawSelectAllNoneButtons(IEnumerable<string> keys, IDictionary<string, bool> selection)
        {
            var keyList = keys.ToList();
            EditorGUILayout.BeginHorizontal();
            if (GUILayout.Button("All", GUILayout.Width(50)))
                foreach (var key in keyList) selection[key] = true;
            if (GUILayout.Button("None", GUILayout.Width(50)))
                foreach (var key in keyList) selection[key] = false;
            EditorGUILayout.EndHorizontal();
        }

        private void Export(List<Locale> locales, List<StringTableCollection> collections)
        {
            EditorPrefs.SetString(OutputFolderPrefKey, _outputFolder);
            EditorPrefs.SetBool(IncludeIdPrefKey, _includeId);
            EditorPrefs.SetBool(IncludeCommentsPrefKey, _includeComments);

            Directory.CreateDirectory(_outputFolder);

            // Description and English always come first, and always exist as a column even if a
            // collection has no String Table for them (or the table is entirely empty), so every
            // exported file has the same, predictable column layout.
            var columnLocales = new List<Locale>();
            if (_descriptionLocale != null) columnLocales.Add(_descriptionLocale);
            if (_englishLocale != null) columnLocales.Add(_englishLocale);
            columnLocales.AddRange(locales);

            foreach (var collection in collections)
                ExportCollection(collection, columnLocales);

            EditorUtility.RevealInFinder(_outputFolder);
            Debug.Log($"Exported {collections.Count} CSV file(s), {columnLocales.Count} locale column(s) each, to '{_outputFolder}'.");
        }

        private void ExportCollection(StringTableCollection collection, List<Locale> columnLocales)
        {
            var tablesByLocaleCode = collection.StringTables
                .Where(t => t != null)
                .ToDictionary(t => t.LocaleIdentifier.Code, t => t);

            var filePath = Path.Combine(_outputFolder, SanitizeFileName(collection.TableCollectionName) + ".csv");
            // BOM included so Excel correctly detects UTF-8 (accented characters in fr/es/etc. otherwise show as mojibake).
            using var writer = new StreamWriter(filePath, false, new UTF8Encoding(true));

            var header = new List<string> { "Key" };
            if (_includeId) header.Add("Id");
            if (_includeComments) header.Add("Shared Comments");
            foreach (var locale in columnLocales)
            {
                var label = ColumnLabel(locale);
                header.Add(label);
                if (_includeComments) header.Add(label + " Comments");
            }
            WriteRow(writer, header);

            foreach (var keyEntry in collection.SharedData.Entries)
            {
                var row = new List<string> { keyEntry.Key };
                if (_includeId) row.Add(keyEntry.Id.ToString());
                if (_includeComments) row.Add(keyEntry.Metadata.GetMetadata<Comment>()?.CommentText ?? string.Empty);

                foreach (var locale in columnLocales)
                {
                    tablesByLocaleCode.TryGetValue(locale.Identifier.Code, out var table);
                    var entry = table?.GetEntry(keyEntry.Id);
                    row.Add(entry?.LocalizedValue ?? string.Empty);
                    if (_includeComments)
                        row.Add(entry?.GetMetadata<Comment>()?.CommentText ?? string.Empty);
                }

                WriteRow(writer, row);
            }
        }

        private static string ColumnLabel(Locale locale)
        {
            // locale.LocaleName already includes the code, e.g. "Spanish (es)" - don't append it again.
            return locale.Identifier.Code == DescriptionLocaleCode ? "Description" : locale.LocaleName;
        }

        private static void WriteRow(TextWriter writer, IEnumerable<string> fields)
        {
            writer.WriteLine(string.Join(",", fields.Select(EscapeCsvField)));
        }

        private static string EscapeCsvField(string field)
        {
            field ??= string.Empty;
            if (field.IndexOfAny(new[] { ',', '"', '\n', '\r' }) < 0)
                return field;
            return "\"" + field.Replace("\"", "\"\"") + "\"";
        }

        private static string SanitizeFileName(string name)
        {
            foreach (var invalidChar in Path.GetInvalidFileNameChars())
                name = name.Replace(invalidChar, '_');
            return name;
        }
    }
}
