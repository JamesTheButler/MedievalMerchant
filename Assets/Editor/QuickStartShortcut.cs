using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Editor
{
    public sealed class QuickStartShortcut
    {
        private const string MainScenePath = "Assets/Scenes/StartScene.unity";

        [Shortcut("Tools/Play From Main Scene", KeyCode.F5)]
        private static void PlayFromMainSceneShortcut()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            if (EditorSceneManager.GetActiveScene().path != MainScenePath)
            {
                if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                {
                    return;
                }

                var scene = EditorSceneManager.OpenScene(MainScenePath, OpenSceneMode.Single);
                if (!scene.IsValid())
                {
                    Debug.LogError($"Could not open scene at '{MainScenePath}'. Check the path.");
                    return;
                }
            }

            EditorApplication.isPlaying = true;
        }
    }
}