using System.Collections.Generic;
using System.IO;
using Features.Combat.UI;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

namespace Editor
{
    /// <summary>
    /// Accessed by cloud to convert design pages from other tools into Unity uGUI.
    /// </summary>
    public static class AiBasedUiComponentGenerator
    {
        private const string TargetFolder = "Assets/Features/Combat/UI";
        private const string LocalizedTextPrefab = "Assets/Features/Localization/UI/LocalizedText.prefab";

        private const int TitleStyle = 97690656, SubtitleStyle = 2085476100;

        private const int IconSize = 32;
        private const int CharacterSize = 64;
        private const int DefaultPaddingSmall = 4;
        private const int DefaultPaddingMedium = 8;
        private const int DefaultPaddingLarge = 16;

        [MenuItem("Tools/Combat/Generate Atom Prefabs")]
        public static void Generate()
        {
            Directory.CreateDirectory(TargetFolder);

            var built = new List<string>
            {
                BuildDemoUIComponent()
            };

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log("Combat atoms generated:\n - " + string.Join("\n - ", built));
        }

        // ----------------------------------------------------------------- atoms

        private static string BuildDemoUIComponent()
        {
            var root = NewUI("UnitToken");
            SetSize(root, 64, 64);

            // 1. background icon, radial-filled to current health
            var healthFill = NewIcon("HealthFill", root, 64);
            healthFill.type = Image.Type.Filled;
            healthFill.fillMethod = Image.FillMethod.Radial360;
            healthFill.fillOrigin = (int)Image.Origin360.Top;
            healthFill.fillClockwise = true;
            healthFill.fillAmount = 1f;

            // 2. the guard or bandit icon on top
            var character = NewIcon("Character", root, CharacterSize);

            // 3. the blinker, flashed on hit
            var blinker = NewIcon("Blinker", root, IconSize);
            blinker.gameObject.SetActive(false);

            var canvasGroup = root.AddComponent<CanvasGroup>();
            var handler = root.AddComponent<UnitTokenTooltipHandler>();
            var component = root.AddComponent<UnitToken>();

            Assign(component, new Dictionary<string, Object>
            {
                { "healthFill", healthFill },
                { "character", character },
                { "blinker", blinker.gameObject },
                { "canvasGroup", canvasGroup },
                { "tooltipHandler", handler }
            });

            return Save(root);
        }


        // ----------------------------------------------------------------- helpers
        private static GameObject NewUI(string name)
        {
            return new GameObject(name, typeof(RectTransform));
        }

        private static GameObject NewRow(string name, float spacing)
        {
            var go = NewUI(name);

            var layout = go.AddComponent<HorizontalLayoutGroup>();
            layout.spacing = spacing;
            layout.childAlignment = TextAnchor.MiddleLeft;
            layout.childForceExpandWidth = false;
            layout.childForceExpandHeight = false;
            layout.childControlWidth = true;
            layout.childControlHeight = true;

            Fit(go);
            return go;
        }

        private static void Fit(GameObject go)
        {
            var fitter = go.AddComponent<ContentSizeFitter>();
            fitter.horizontalFit = ContentSizeFitter.FitMode.PreferredSize;
            fitter.verticalFit = ContentSizeFitter.FitMode.PreferredSize;
        }

        private static Image NewIcon(string name, GameObject parent, int size)
        {
            var go = NewUI(name);
            go.transform.SetParent(parent.transform, false);

            var image = go.AddComponent<Image>();
            image.raycastTarget = false;

            SetSize(go, size, size);

            var element = go.AddComponent<LayoutElement>();
            element.preferredWidth = size;
            element.preferredHeight = size;

            return image;
        }

        private static TMP_Text NewText(string name, GameObject parent, float fontSize, FontStyles style = FontStyles.Normal)
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

        // Static text goes in as a LocalizedText instance rather than a bare TMP field,
        // so it can never carry a hardcoded string. The LocalizedString it points at is
        // left unassigned - that is the separate localisation pass.
        private static TMP_Text NewLocalizedText(string name, GameObject parent)
        {
            var asset = AssetDatabase.LoadAssetAtPath<GameObject>(LocalizedTextPrefab);

            if (asset == null)
            {
                Debug.LogError("LocalizedText prefab not found at " + LocalizedTextPrefab);
                return NewText(name, parent, 24f);
            }

            var instance = (GameObject)PrefabUtility.InstantiatePrefab(asset, parent.transform);
            instance.name = name;

            return instance.GetComponent<TMP_Text>();
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

        private static void SetSize(GameObject go, float width, float height)
        {
            ((RectTransform)go.transform).sizeDelta = new Vector2(width, height);
        }

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

        private static string Save(GameObject root)
        {
            var path = TargetFolder + "/" + root.name + ".prefab";

            PrefabUtility.SaveAsPrefabAsset(root, path);
            Object.DestroyImmediate(root);

            return path;
        }
    }
}
