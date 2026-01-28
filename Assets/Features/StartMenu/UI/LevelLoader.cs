using Common.Infrastructure.Global;
using Features.Levels;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Features.StartMenu.UI
{
    public sealed class LevelLoader : MonoBehaviour
    {
        [SerializeField, Scene]
        private string gameScene;

        public void LoadLevel(LevelInfo levelInfo)
        {
            Debug.Log($"Loading level {levelInfo.LevelName}...");
            GlobalContext.CurrentLevelInfo = levelInfo;
            SceneManager.LoadScene(gameScene);
        }
    }
}