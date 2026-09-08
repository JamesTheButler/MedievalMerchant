using System.Collections.Generic;
using System.IO;
using System.Linq;
using Features.Localization.UI;
using TMPro;
using UnityEditor;
using UnityEditor.Localization;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

namespace Editor
{
    /// <summary>
    /// Accessed by claude to convert design pages from other tools into Unity uGUI.
    /// Driven by the `ui-development` skill: it writes builder methods here, the user runs
    /// the menu item below, and Unity's own API produces the prefabs.
    ///
    /// Builder methods are single-use. Old ones are deleted outright when a new UI is
    /// generated - the prefabs they produced stay on disk and are hand-tuned from there.
    /// The constants block is the exception: it must be preserved verbatim.
    /// </summary>
    public static class AiBasedUiComponentGenerator
    {
        // ----------------------------------------------------------------- constants
        // Do not change these. They are the project's design tokens.

        // Styles, as TMP serializes them - hashes into the default style sheet.
        private const int StyleTitle = 97690656,
            StyleSubtitle = 2085476100,
            StyleGood = 1913603873,
            StyleBad = -1243527163;

        private const int DefaultIconSize = 32;
        private const int SpecialIconSize = 64;
        private const int PaddingSmall = 4;
        private const int PaddingMedium = 8;
        private const int PaddingLarge = 12;

        private const string LocalizedTextPrefab = "Assets/Features/Localization/UI/LocalizedText.prefab";
        private const string UiElementsPath = "Assets/Common/UI/Art/UiElements.png";

        // The source-of-truth locale, and the pseudo-locale holding translator descriptions.
        // Both match Assets/Editor/LocalizationCsvExporterWindow.cs.
        private const string EnglishLocaleCode = "en";
        private const string CommentLocaleCode = "comment";

        // ----------------------------------------------------------------- per run
        // Point this at the UI folder of the feature currently being generated,
        // e.g. "Assets/Features/Towns/UI".

        private const string TargetFolder = "Assets/Features";

        [MenuItem("Tools/AI/Generate UI Prefabs")]
        public static void Generate()
        {
            var built = new List<string>();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(built.Count == 0
                ? "No builder methods registered - add them to Generate()."
                : "UI prefabs generated:\n - " + string.Join("\n - ", built));
        }

        // ----------------------------------------------------------------- example
        // Not registered in Generate(). Kept as the canonical shape of a builder method:
        // nested panels for the visual block, a layout-only row inside them, an icon, a
        // code-driven number, and a static localized label.
        //
        // A real builder ends by adding its code-behind component and calling Assign(...)
        // with that component's exact [SerializeField] field names.

        private static string BuildExampleElement()
        {
            var root = NewPanel("ExampleElement", null, PanelKind.Background);
            SetSize(root, 220, 48);

            var inner = NewPanel("Inner", root, PanelKind.Foreground);
            Stretch(inner, PaddingSmall);

            var row = NewRow("Row", PaddingMedium);
            row.transform.SetParent(inner.transform, false);

            NewIcon("Icon", row, DefaultIconSize);

            // Code-driven text: a plain TMP field, with a realistic placeholder so the
            // prefab reads correctly in the editor.
            var amount = NewText("Amount", row, 24f);
            amount.text = "1,240";
            SetTextStyle(amount, StyleTitle);

            // Static text: never a bare TMP field. The entry is created and wired here, so
            // the label shows real English in the editor instead of "<none>".
            NewLocalizedText(
                "Label", row,
                table: "Common",
                key: "Common.Example.Label",
                english: "in stock",
                comment: "Suffix after a goods count, e.g. \"1,240 in stock\".",
                styleHashCode: StyleSubtitle);

            return Save(root);
        }

        // ----------------------------------------------------------------- panels

        public enum PanelKind
        {
            Background,
            Foreground,
            Good,
            Bad,
            BackgroundSquare,
            ForegroundSquare,
            TextField,
            TopBackground,
            TopForeground,
            Button,
        }

        // Every visual block in this project is an alternately nested Background /
        // Foreground panel. The sprites carry 9-slice borders but are drawn Tiled, matching
        // Panel UI Template.prefab - Sliced would stretch the border art.
        private static GameObject NewPanel(string name, GameObject parent, PanelKind kind)
        {
            var go = NewUI(name);

            if (parent != null)
            {
                go.transform.SetParent(parent.transform, false);
            }

            var image = go.AddComponent<Image>();
            image.sprite = LoadUiSprite(SpriteName(kind));
            image.type = Image.Type.Tiled;
            image.pixelsPerUnitMultiplier = 1f;

            return go;
        }

        private static string SpriteName(PanelKind kind)
        {
            return kind switch
            {
                PanelKind.Background => "PanelBackground",
                PanelKind.Foreground => "PanelForeground",
                PanelKind.Good => "PanelForegroundGood",
                PanelKind.Bad => "PanelForegroundBad",
                PanelKind.BackgroundSquare => "PanelBackgroundSquare",
                PanelKind.ForegroundSquare => "PanelForegroundSquare",
                PanelKind.TextField => "PanelTextField",
                PanelKind.TopBackground => "TopPanelBackground",
                PanelKind.TopForeground => "TopPanelForeground",
                PanelKind.Button => "Button",
                _ => "PanelBackground",
            };
        }

        // UiElements.png is a multi-sprite sheet, so LoadAssetAtPath returns the texture
        // rather than any one sprite - sub-sprites have to be filtered out by name.
        private static Sprite LoadUiSprite(string spriteName)
        {
            var sprite = AssetDatabase.LoadAllAssetsAtPath(UiElementsPath)
                .OfType<Sprite>()
                .FirstOrDefault(s => s.name == spriteName);

            if (sprite == null)
            {
                Debug.LogError("Sub-sprite not found in " + UiElementsPath + ": " + spriteName);
            }

            return sprite;
        }

        private static Button NewButton(string name, GameObject parent, float width, float height)
        {
            var go = NewPanel(name, parent, PanelKind.Button);
            SetSize(go, width, height);

            var button = go.AddComponent<Button>();
            button.targetGraphic = go.GetComponent<Image>();

            return button;
        }

        // ----------------------------------------------------------------- layout

        private static GameObject NewUI(string name)
        {
            return new GameObject(name, typeof(RectTransform));
        }

        private static GameObject NewRow(string name, float spacing)
        {
            return NewLayoutGroup<HorizontalLayoutGroup>(name, spacing, TextAnchor.MiddleLeft);
        }

        private static GameObject NewColumn(string name, float spacing)
        {
            return NewLayoutGroup<VerticalLayoutGroup>(name, spacing, TextAnchor.UpperLeft);
        }

        // A row/column is a layout-only object: RectTransform plus the layout group, no
        // Image and no CanvasRenderer. Its children do the rendering.
        private static GameObject NewLayoutGroup<TGroup>(string name, float spacing, TextAnchor alignment)
            where TGroup : HorizontalOrVerticalLayoutGroup
        {
            var go = NewUI(name);

            var layout = go.AddComponent<TGroup>();
            layout.spacing = spacing;
            layout.childAlignment = alignment;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = true;
            layout.childControlHeight = true;

            Fit(go);
            return go;
        }

        private static void SetPadding(GameObject go, int padding)
        {
            var layout = go.GetComponent<LayoutGroup>();

            if (layout == null)
            {
                Debug.LogError("No layout group on " + go.name);
                return;
            }

            layout.padding = new RectOffset(padding, padding, padding, padding);
        }

        private static void Fit(GameObject go)
        {
            var fitter = go.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        // Fills the parent rect, optionally inset on every side.
        private static void Stretch(GameObject go, float inset = 0f)
        {
            var rect = (RectTransform)go.transform;

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = new Vector2(inset, inset);
            rect.offsetMax = new Vector2(-inset, -inset);
        }

        private static void SetSize(GameObject go, float width, float height)
        {
            ((RectTransform)go.transform).sizeDelta = new Vector2(width, height);
        }

        private static Image NewIcon(string name, GameObject parent, int size)
        {
            var go = NewUI(name);
            go.transform.SetParent(parent.transform, false);

            var image = go.AddComponent<Image>();
            image.raycastTarget = false;

            SetSize(go, size, size);

            // Inside a layout group the LayoutElement is what actually holds the size.
            var element = go.AddComponent<LayoutElement>();
            element.preferredWidth = size;
            element.preferredHeight = size;

            return image;
        }

        // ----------------------------------------------------------------- text

        private static TMP_Text NewText(string name, GameObject parent, float fontSize,
            FontStyles style = FontStyles.Normal)
        {
            var go = NewUI(name);
            go.transform.SetParent(parent.transform, false);

            var text = go.AddComponent<TextMeshProUGUI>();
            text.fontSize = fontSize;
            text.fontStyle = style;
            text.alignment = TextAlignmentOptions.Left;
            text.textWrappingMode = TextWrappingModes.NoWrap;
            text.raycastTarget = false;
            text.text = name;

            return text;
        }

        // Static text goes in as a LocalizedText instance rather than a bare TMP field, so
        // it can never carry a hardcoded string. The entry is created and the key wired up
        // here - LocalizedText overwrites its own text on OnEnable/OnValidate and would
        // otherwise render "<none>" in the editor.
        private static LocalizedText NewLocalizedText(
            string name,
            GameObject parent,
            string table,
            string key,
            string english,
            string comment,
            bool isSmart = false,
            int styleHashCode = StyleTitle)
        {
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(LocalizedTextPrefab);

            if (asset == null)
            {
                Debug.LogError("LocalizedText prefab not found at " + LocalizedTextPrefab);
                return null;
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(asset, parent.transform);
            instance.name = name;

            var localizedText = instance.GetComponent<LocalizedText>();
            AssignLocalizedString(localizedText, "staticString", table, key, english, comment, isSmart);

            SetTextStyle(instance.GetComponent<TMP_Text>(), styleHashCode);

            return localizedText;
        }

        // TMP exposes the style only as a serialized hash, so it is written the same way
        // the prefab YAML stores it.
        private static void SetTextStyle(TMP_Text text, int styleHashCode)
        {
            var serialized = new SerializedObject(text);
            var property = serialized.FindProperty("m_TextStyleHashCode");

            if (property == null)
            {
                Debug.LogError("No m_TextStyleHashCode on " + text.name);
                return;
            }

            property.intValue = styleHashCode;
            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        // ----------------------------------------------------------------- localization

        /// <summary>
        /// Creates (or updates) one string table entry and returns its key id. Never creates
        /// a table collection - a missing one is an error for the user to fix in the
        /// Localization Tables window.
        /// </summary>
        private static long EnsureEntry(string table, string key, string english, string comment, bool isSmart)
        {
            var collection = LocalizationEditorSettings.GetStringTableCollection(table);

            if (collection == null)
            {
                Debug.LogError(
                    "String table collection not found: " + table +
                    ". Create it in Window/Asset Management/Localization Tables - this generator never creates tables.");
                return 0;
            }

            var shared = collection.SharedData;
            var sharedEntry = shared.GetEntry(key) ?? shared.AddKey(key);

            WriteEntry(collection, EnglishLocaleCode, sharedEntry.Id, english, isSmart);
            WriteEntry(collection, CommentLocaleCode, sharedEntry.Id, comment, false);

            EditorUtility.SetDirty(shared);
            EditorUtility.SetDirty(collection);

            return sharedEntry.Id;
        }

        private static void WriteEntry(
            StringTableCollection collection, string localeCode, long keyId, string value, bool isSmart)
        {
            if (string.IsNullOrEmpty(value))
                return;

            var table = collection.StringTables.FirstOrDefault(t => t.LocaleIdentifier.Code == localeCode);

            if (table == null)
            {
                Debug.LogError(collection.TableCollectionName + " has no '" + localeCode + "' table.");
                return;
            }

            var entry = table.GetEntry(keyId) ?? table.AddEntry(keyId, value);
            entry.Value = value;
            entry.IsSmart = isSmart;

            EditorUtility.SetDirty(table);
        }

        /// <summary>
        /// Points a serialized LocalizedString field at a table entry - LocalizedText's own
        /// `staticString`, or a [SerializeField] LocalizedString on a code-behind.
        /// </summary>
        private static void AssignLocalizedString(Component component, string fieldName, string table, long keyId)
        {
            if (component == null || keyId == 0)
                return;

            var collection = LocalizationEditorSettings.GetStringTableCollection(table);

            if (collection == null)
                return;

            var serialized = new SerializedObject(component);
            var tableProperty = serialized.FindProperty(fieldName + ".m_TableReference.m_TableCollectionName");
            var keyIdProperty = serialized.FindProperty(fieldName + ".m_TableEntryReference.m_KeyId");
            var keyProperty = serialized.FindProperty(fieldName + ".m_TableEntryReference.m_Key");

            if (tableProperty == null || keyIdProperty == null)
            {
                Debug.LogError(component.GetType().Name + " has no LocalizedString field: " + fieldName);
                return;
            }

            tableProperty.stringValue = "GUID:" + collection.SharedData.TableCollectionNameGuid.ToString("N");
            keyIdProperty.longValue = keyId;

            if (keyProperty != null)
            {
                keyProperty.stringValue = string.Empty;
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        /// <summary>
        /// Creates the entry and assigns it to a [SerializeField] LocalizedString in one step.
        /// </summary>
        private static void AssignLocalizedString(
            Component component, string fieldName, string table, string key,
            string english, string comment, bool isSmart = false)
        {
            var keyId = EnsureEntry(table, key, english, comment, isSmart);
            AssignLocalizedString(component, fieldName, table, keyId);
        }

        // ----------------------------------------------------------------- wiring

        // Binds by field-name string: a name that does not match the C# field logs an error
        // and leaves the field unassigned rather than failing to compile. Copy the names out
        // of the code-behind.
        private static void Assign(Component component, Dictionary<string, Object> fields)
        {
            var serialized = new SerializedObject(component);

            foreach (var field in fields)
            {
                var property = serialized.FindProperty(field.Key);

                if (property == null)
                {
                    Debug.LogError(component.GetType().Name + " has no serialized field: " + field.Key);
                    continue;
                }

                property.objectReferenceValue = field.Value;
            }

            serialized.ApplyModifiedPropertiesWithoutUndo();
        }

        // The prefab filename is the root GameObject's name is the code-behind class name.
        private static string Save(GameObject root)
        {
            Directory.CreateDirectory(TargetFolder);

            var path = TargetFolder + "/" + root.name + ".prefab";

            PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);

            return path;
        }
    }
}
