using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using static Editor.UIGenerationHelper;

namespace Editor
{
    /// <summary>
    /// Single-use builder methods, written by the `ui-development` skill and empty between
    /// passes. See that skill for how and when to rewrite this file.
    /// </summary>
    public static class UIGenerator
    {
        // The feature's UI folder: where Save() writes, and where NewElement<T>() and
        // LoadElement<T>() find prefabs an earlier pass already built.
        private const string OutputFolder = "Assets/Features";

        [MenuItem("Tools/AI/Generate UI Prefabs")]
        public static void Generate()
        {
            TargetFolder = OutputFolder;

            var built = new List<string>();

            AssetDatabase.SaveAssets();
            AssetDatabase.Refresh();

            Debug.Log(built.Count == 0
                ? "No builder methods registered - add them to Generate()."
                : "UI prefabs generated:\n - " + string.Join("\n - ", built));
        }
    }
}
