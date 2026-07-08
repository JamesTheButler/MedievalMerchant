using UnityEditor;

namespace Editor
{
    public static class SelectLocalizationResources
    {
        private const string AssetPath = "Assets/Features/Localization/Data/LocalizationResources.asset";

        [MenuItem("Tools/Localization/Select Localization Resources %#l")]
        private static void Select()
        {
            var asset = AssetDatabase.LoadAssetAtPath<UnityEngine.Object>(AssetPath);
            Selection.activeObject = asset;
            EditorGUIUtility.PingObject(asset);
        }
    }
}
