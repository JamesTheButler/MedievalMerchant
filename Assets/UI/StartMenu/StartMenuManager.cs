using Common;
using Levels;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

namespace UI.StartMenu
{
    public sealed class StartMenuManager : MonoBehaviour
    {
        [SerializeField, Scene]
        private string gameScene;
        
        [SerializeField, Required]
        private Animation logoAnimation;

        [SerializeField, Required]
        private TMP_Text pressAnyText;

        [SerializeField, Required]
        private GameObject levelButtonGroup;

        [SerializeField, Required]
        private LevelInfoBox levelInfoBox;

        private bool _initialized;

        public void LoadLevel(LevelInfo levelInfo)
        {
            Debug.Log($"Loading level {levelInfo.LevelName}...");

            SceneManager.LoadScene(gameScene);
        }

        private void Start()
        {
            levelButtonGroup.SetActive(false);
            pressAnyText.gameObject.SetActive(true);
            levelInfoBox.Clear();
        }

        private void Update()
        {
            if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.WasAnyKeyPressedThisFrame())
            {
                OnAnyKey();
            }
        }

        private void OnAnyKey()
        {
            if (_initialized) return;

            logoAnimation.Play();
            pressAnyText.gameObject.SetActive(false);
            levelButtonGroup.SetActive(true);

            _initialized = true;
        }

        public void SetupLevelInfoBox(LevelInfo levelInfo)
        {
            levelInfoBox.Setup(levelInfo);
        }

        public void ClearLevelInfoBox()
        {
            levelInfoBox.Clear();
        }
    }
}