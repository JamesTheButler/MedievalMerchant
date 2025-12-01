using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEditor.ShortcutManagement;
using UnityEngine;

namespace Editor
{
    public sealed class SceneEditorShortcuts
    {
        private const string StartScenePath = "Assets/Scenes/StartScene.unity";
        private const string GameplayScenePath = "Assets/Scenes/GameplayScene.unity";

        [Shortcut("Tools/Play From Main Scene", KeyCode.F5)]
        private static void PlayFromMainSceneShortcut()
        {
            if (EditorApplication.isPlaying)
            {
                EditorApplication.isPlaying = false;
                return;
            }

            SwitchScene(StartScenePath);

            EditorApplication.isPlaying = true;
        }

        [Shortcut("Tools/Switch between Gameplay and Start Scene", KeyCode.F6)]
        private static void SwitchScenes()
        {
            var activeScene = EditorSceneManager.GetActiveScene().path;
            var nextScene = activeScene != GameplayScenePath ? GameplayScenePath : StartScenePath;
            SwitchScene(nextScene);
        }

        private static void SwitchScene(string scenePath)
        {
            if (EditorSceneManager.GetActiveScene().path == scenePath)
                return;

            if (!EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo())
                return;

            var scene = EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single);
            if (!scene.IsValid())
            {
                Debug.LogError($"Could not open scene at '{scenePath}'. Check the path.");
            }
        }
    }
}