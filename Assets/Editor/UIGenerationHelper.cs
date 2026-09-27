using System.Collections.Generic;
using System.IO;
using System.Linq;
using Features.Localization.UI;
using TMPro;
using UnityEditor;
using UnityEditor.Localization;
using UnityEditorInternal;
using UnityEngine;
using UnityEngine.Localization.Tables;
using UnityEngine.UI;

namespace Editor
{
    /// <summary>
    /// The stable half of AI-driven UI generation: design tokens, layout and panel helpers,
    /// the shared button prefabs, localization entry creation, and field wiring.
    ///
    /// <see cref="UIGenerator"/> is the half that gets rewritten for each new screen. Nothing
    /// screen-specific belongs in this file - if a builder needs a new shape, add the helper
    /// here rather than inlining the same six lines into three builders.
    ///
    /// Consumed with <c>using static Editor.UIGenerationHelper;</c> so builder methods read
    /// unqualified.
    /// </summary>
    public static class UIGenerationHelper
    {
        // ----------------------------------------------------------------- constants
        // Do not change these. They are the project's design tokens.

        // Styles, as TMP serializes them - hashes into the default style sheet.
        public const int StyleTitle = 97690656,
            StyleSubtitle = 2085476100,
            StyleGood = 1913603873,
            StyleBad = -1243527163;

        public const int DefaultIconSize = 32;
        public const int SpecialIconSize = 64;
        public const int PaddingSmall = 4;
        public const int PaddingMedium = 8;
        public const int PaddingLarge = 12;

        private const string LocalizedTextPrefab = "Assets/Features/Localization/UI/LocalizedText.prefab";
        private const string UiElementsPath = "Assets/Common/UI/Art/UiElements.png";

        // Shared button prefabs. Never build a button out of a panel and a Button component -
        // these carry the project's hover, layout and text conventions.
        private const string ButtonPrefab = "Assets/Common/UI/Elements/Button.prefab";
        private const string IconButtonPrefab = "Assets/Common/UI/Elements/ButtonWithIcon.prefab";
        private const string CloseButtonPrefab = "Assets/Common/UI/Elements/XButton.prefab";

        // Child object names inside those prefabs.
        private const string ButtonTextChild = "Button Text";
        private const string ButtonIconChild = "Button Icon";

        // The source-of-truth locale, and the pseudo-locale holding translator descriptions.
        // Both match Assets/Editor/LocalizationCsvExporterWindow.cs.
        private const string EnglishLocaleCode = "en";
        private const string CommentLocaleCode = "comment";

        // ----------------------------------------------------------------- output folder

        /// <summary>
        /// Where <see cref="Save"/> writes, and where <see cref="NewElement{TElement}"/> looks for
        /// the layer-1 element prefabs. Set once by <see cref="UIGenerator"/> before it builds.
        /// </summary>
        public static string TargetFolder { get; set; } = "Assets/Features";

        // ----------------------------------------------------------------- components

        /// <summary>
        /// Adds a component and moves it directly under the Transform, so hand-editing a
        /// generated prefab starts with the code-behind rather than scrolling past an Image,
        /// a layout group and a fitter. Use this for every code-behind; plain
        /// <c>AddComponent</c> is still right for the uGUI plumbing that should sit below it.
        /// </summary>
        public static TComponent AddBehaviour<TComponent>(GameObject go) where TComponent : Component
        {
            var component = go.AddComponent<TComponent>();
            MoveToTop(component);
            return component;
        }

        /// <summary>
        /// Walks a component up the inspector until only the Transform is above it.
        /// </summary>
        public static void MoveToTop(Component component)
        {
            if (component == null)
                return;

            while (IndexOf(component) > 1 && ComponentUtility.MoveComponentUp(component))
            {
            }
        }

        private static int IndexOf(Component component)
        {
            return System.Array.IndexOf(component.gameObject.GetComponents<Component>(), component);
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
        public static GameObject NewPanel(string name, GameObject parent, PanelKind kind)
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

        public static string SpriteName(PanelKind kind)
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
        public static Sprite LoadUiSprite(string spriteName)
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

        // ----------------------------------------------------------------- buttons

        /// <summary>
        /// A labelled button, from the shared Button prefab. Its label is a LocalizedText, so
        /// the entry is created and wired here the same way <see cref="NewLocalizedText"/> does.
        /// Pass an <paramref name="icon"/> to show the prefab's icon slot; leave it null and the
        /// slot is switched off. <paramref name="iconAfterText"/> flips the row's
        /// Reverse Arrangement so the icon trails the label.
        /// </summary>
        public static Button NewButton(
            string name,
            GameObject parent,
            string table,
            string key,
            string english,
            string comment,
            Sprite icon = null,
            bool iconAfterText = false)
        {
            var button = NewButtonInstance(ButtonPrefab, name, parent);

            if (button == null)
                return null;

            AssignButtonLabel(button, table, key, english, comment);
            SetButtonIcon(button, icon);

            var layout = button.GetComponent<HorizontalLayoutGroup>();

            if (layout != null)
            {
                layout.reverseArrangement = iconAfterText;
            }

            return button;
        }

        /// <summary>
        /// An icon-only button, from the shared ButtonWithIcon prefab.
        /// </summary>
        public static Button NewIconButton(string name, GameObject parent, Sprite icon)
        {
            var button = NewButtonInstance(IconButtonPrefab, name, parent);

            if (button == null)
                return null;

            SetButtonIcon(button, icon);
            return button;
        }

        /// <summary>
        /// The small X that closes a panel. Belongs in the top-right corner; it carries its own
        /// glyph, so there is nothing to localize.
        /// </summary>
        public static Button NewCloseButton(string name, GameObject parent)
        {
            return NewButtonInstance(CloseButtonPrefab, name, parent);
        }

        private static Button NewButtonInstance(string prefabPath, string name, GameObject parent)
        {
            var instance = NewPrefabInstance(prefabPath, name, parent);

            if (instance == null)
                return null;

            var button = instance.GetComponent<Button>();

            if (button == null)
            {
                Debug.LogError("No Button component on " + prefabPath);
            }

            return button;
        }

        private static void AssignButtonLabel(
            Button button, string table, string key, string english, string comment)
        {
            var label = FindChild(button.gameObject, ButtonTextChild);

            if (label == null)
                return;

            var localizedText = label.GetComponent<LocalizedText>();

            if (localizedText == null)
            {
                Debug.LogError("No LocalizedText on " + ButtonTextChild + " of " + button.name);
                return;
            }

            AssignLocalizedString(localizedText, "staticString", table, key, english, comment);
        }

        private static void SetButtonIcon(Button button, Sprite icon)
        {
            var iconObject = FindChild(button.gameObject, ButtonIconChild);

            if (iconObject == null)
                return;

            var image = iconObject.GetComponent<Image>();

            if (image != null)
            {
                image.sprite = icon;
            }

            iconObject.SetActive(icon != null);
        }

        private static GameObject FindChild(GameObject go, string childName)
        {
            var child = go.transform.Find(childName);

            if (child == null)
            {
                Debug.LogError("No child named '" + childName + "' under " + go.name);
                return null;
            }

            return child.gameObject;
        }

        // ----------------------------------------------------------------- prefab instances

        public static GameObject NewPrefabInstance(string prefabPath, string name, GameObject parent)
        {
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(prefabPath);

            if (asset == null)
            {
                Debug.LogError("Prefab not found: " + prefabPath);
                return null;
            }

            var parentTransform = parent == null ? null : parent.transform;
            var instance = (GameObject)PrefabUtility.InstantiatePrefab(asset, parentTransform);
            instance.name = name;

            return instance;
        }

        // Layer-1 element prefabs live in TargetFolder under their own class name, which is what
        // makes this lookup safe: the prefab filename is the root GameObject's name is the class
        // name, and Save() enforces that.
        public static string ElementPath<TElement>() where TElement : Component
        {
            return TargetFolder + "/" + typeof(TElement).Name + ".prefab";
        }

        public static TElement LoadElement<TElement>() where TElement : Component
        {
            var path = ElementPath<TElement>();
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(path);

            if (asset == null)
            {
                Debug.LogError("Element prefab not found: " + path);
                return null;
            }

            return asset.GetComponent<TElement>();
        }

        public static TElement NewElement<TElement>(string name, GameObject parent)
            where TElement : Component
        {
            var instance = NewPrefabInstance(ElementPath<TElement>(), name, parent);
            return instance == null ? null : instance.GetComponent<TElement>();
        }

        // ----------------------------------------------------------------- layout

        public static GameObject NewUI(string name)
        {
            return new GameObject(name, typeof(RectTransform));
        }

        public static GameObject NewRow(string name, float spacing,
            TextAnchor alignment = TextAnchor.MiddleLeft)
        {
            return NewLayoutGroup<HorizontalLayoutGroup>(name, spacing, alignment);
        }

        public static GameObject NewColumn(string name, float spacing,
            TextAnchor alignment = TextAnchor.UpperLeft)
        {
            return NewLayoutGroup<VerticalLayoutGroup>(name, spacing, alignment);
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

        public static void SetPadding(GameObject go, int padding)
        {
            var layout = go.GetComponent<LayoutGroup>();

            if (layout == null)
            {
                Debug.LogError("No layout group on " + go.name);
                return;
            }

            layout.padding = new RectOffset(padding, padding, padding, padding);
        }

        public static void SetPaddingLeft(GameObject go, int padding)
        {
            var layout = go.GetComponent<LayoutGroup>();

            if (layout == null)
            {
                Debug.LogError("No layout group on " + go.name);
                return;
            }

            layout.padding.left = padding;
        }

        public static void Fit(GameObject go)
        {
            var fitter = go.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        /// <summary>
        /// Releases a layout container's horizontal fit so the width can be pinned, while the
        /// height still grows with its content.
        /// </summary>
        public static void FixedWidth(GameObject go, float width)
        {
            var fitter = go.GetComponent<ContentSizeFitter>();

            if (fitter != null)
            {
                fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            }

            var rect = (RectTransform)go.transform;
            rect.sizeDelta = new Vector2(width, rect.sizeDelta.y);

            // Not ?? - GetComponent returns a fake-null the null-coalescing operator cannot
            // see through.
            var element = go.GetComponent<LayoutElement>();

            if (element == null)
            {
                element = go.AddComponent<LayoutElement>();
            }

            element.preferredWidth = width;
        }

        /// <summary>
        /// Drops a container's ContentSizeFitter out of the way, for a rect that is sized by
        /// its anchors rather than by its contents.
        /// </summary>
        public static void Unfit(GameObject go)
        {
            var fitter = go.GetComponent<ContentSizeFitter>();

            if (fitter == null)
                return;

            fitter.horizontalFit = ContentSizeFitter.FitMode.Unconstrained;
            fitter.verticalFit = ContentSizeFitter.FitMode.Unconstrained;
        }

        /// <summary>
        /// Pins a fixed-width column against the parent's left edge, full height.
        /// </summary>
        public static void StretchLeftEdge(GameObject go, float width)
        {
            var rect = (RectTransform)go.transform;

            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(0f, 1f);
            rect.pivot = new Vector2(0f, 0.5f);
            rect.sizeDelta = new Vector2(width, 0f);
            rect.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// Pins a fixed-width column against the parent's right edge, full height.
        /// </summary>
        public static void StretchRightEdge(GameObject go, float width)
        {
            var rect = (RectTransform)go.transform;

            rect.anchorMin = new Vector2(1f, 0f);
            rect.anchorMax = new Vector2(1f, 1f);
            rect.pivot = new Vector2(1f, 0.5f);
            rect.sizeDelta = new Vector2(width, 0f);
            rect.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// A full-width strip along the parent's bottom edge - an underline, a state marker.
        /// </summary>
        public static void StretchBottomEdge(GameObject go, float height)
        {
            var rect = (RectTransform)go.transform;

            rect.anchorMin = new Vector2(0f, 0f);
            rect.anchorMax = new Vector2(1f, 0f);
            rect.pivot = new Vector2(0.5f, 0f);
            rect.sizeDelta = new Vector2(0f, height);
            rect.anchoredPosition = Vector2.zero;
        }

        /// <summary>
        /// A nested Canvas stretched over its parent, for content that is spawned and
        /// destroyed constantly - projectiles, floaters. Keeps their canvas rebuilds off the
        /// rest of the screen.
        /// </summary>
        public static GameObject NewCanvasLayer(string name, GameObject parent, int sortingOrder)
        {
            var go = NewUI(name);
            go.transform.SetParent(parent.transform, false);

            var canvas = go.AddComponent<Canvas>();
            canvas.overrideSorting = true;
            canvas.sortingOrder = sortingOrder;

            Stretch(go);
            return go;
        }

        // Fills the parent rect, optionally inset on every side.
        public static void Stretch(GameObject go, float inset = 0f)
        {
            var rect = (RectTransform)go.transform;

            rect.anchorMin = Vector2.zero;
            rect.anchorMax = Vector2.one;
            rect.pivot = new Vector2(0.5f, 0.5f);
            rect.offsetMin = new Vector2(inset, inset);
            rect.offsetMax = new Vector2(-inset, -inset);
        }

        public static void SetSize(GameObject go, float width, float height)
        {
            ((RectTransform)go.transform).sizeDelta = new Vector2(width, height);
        }

        public static Image NewIcon(string name, GameObject parent, int size)
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

        /// <summary>
        /// A bar fill: stretched over its parent and drawn Filled rather than Tiled, so
        /// fillAmount reads as a share of the parent's width.
        /// </summary>
        public static Image NewFillImage(string name, GameObject parent)
        {
            var go = NewUI(name);
            go.transform.SetParent(parent.transform, false);

            var image = go.AddComponent<Image>();
            image.sprite = LoadUiSprite(SpriteName(PanelKind.ForegroundSquare));
            image.type = Image.Type.Filled;
            image.fillMethod = Image.FillMethod.Horizontal;
            image.fillOrigin = (int)Image.OriginHorizontal.Left;
            image.fillAmount = 0.5f;
            image.raycastTarget = false;

            Stretch(go);

            return image;
        }

        // ----------------------------------------------------------------- text

        public static TMP_Text NewText(string name, GameObject parent, float fontSize,
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
        public static LocalizedText NewLocalizedText(
            string name,
            GameObject parent,
            string table,
            string key,
            string english,
            string comment,
            bool isSmart = false,
            int styleHashCode = StyleTitle)
        {
            var instance = NewPrefabInstance(LocalizedTextPrefab, name, parent);

            if (instance == null)
                return null;

            var localizedText = instance.GetComponent<LocalizedText>();
            AssignLocalizedString(localizedText, "staticString", table, key, english, comment, isSmart);

            SetTextStyle(instance.GetComponent<TMP_Text>(), styleHashCode);

            return localizedText;
        }

        // TMP exposes the style only as a serialized hash, so it is written the same way
        // the prefab YAML stores it.
        public static void SetTextStyle(TMP_Text text, int styleHashCode)
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
        public static long EnsureEntry(string table, string key, string english, string comment, bool isSmart = false)
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
        public static void AssignLocalizedString(Component component, string fieldName, string table, long keyId)
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
        public static void AssignLocalizedString(
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
        public static void Assign(Component component, Dictionary<string, Object> fields)
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
        public static string Save(GameObject root)
        {
            Directory.CreateDirectory(TargetFolder);

            var path = TargetFolder + "/" + root.name + ".prefab";

            PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);

            return path;
        }
    }
}
