using Levels;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.UI;

namespace UI.StartMenu
{
    public sealed class LevelButton : MonoBehaviour
    {
        [SerializeField, Required]
        private StartMenuManager startMenuManager;

        [SerializeField, Required]
        private Button button;

        [SerializeField]
        private LevelInfo levelInfo;

        private void Start()
        {
            if (levelInfo == null)
            {
                button.interactable = false;
            }

            button.onClick.AddListener(OnClick);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            startMenuManager.LoadLevel(levelInfo);
        }
    }
}