using System;
using System.Linq;
using Common;
using Features.Levels.Config;
using Infrastructure;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.StartMenu.UI
{
    public sealed class StartMenuManager : MonoBehaviour
    {
        [SerializeField, Required]
        private Animation logoAnimation;

        [SerializeField, Required]
        private TMP_Text pressAnyText;

        [SerializeField, Required]
        private GameObject startScreenGO, levelSelectionGO;

        [SerializeField, Required]
        private LevelInfoBox levelInfoBox;

        private bool _initialized;
        private LevelButton[] _levelButtons;

        private void Start()
        {
            ToggleLevelSelection(false);
            // set up first level button
            _levelButtons = levelSelectionGO.GetComponentsInChildren<LevelButton>();
            var firstLevelButton = _levelButtons.First();

            OnButtonClick(firstLevelButton);

            // set up click events
            foreach (var button in _levelButtons)
            {
                button.Clicked += OnButtonClick;
            }

            var cursor = ResourceManager.Instance.Cursors.Default;
            Cursor.SetCursor(cursor.Texture, cursor.HotSpot, CursorMode.Auto);
        }

        private void Update()
        {
            // TODO - STYLE: use input system event, i.e. AnyKey through PlayerInput
            if (Keyboard.current.anyKey.wasPressedThisFrame || Mouse.current.WasAnyKeyPressedThisFrame())
            {
                OnAnyKey();
            }
        }

        private void OnButtonClick(LevelButton clickedButton)
        {
            foreach (var otherButton in _levelButtons)
            {
                if (otherButton == clickedButton)
                    continue;

                otherButton.Deselect();
            }

            clickedButton.Select();

            levelInfoBox.Setup(clickedButton.LevelInfo);
        }

        private void OnAnyKey()
        {
            if (_initialized) return;

            logoAnimation.Play();
            pressAnyText.gameObject.SetActive(false);
            ToggleLevelSelection(true);

            _initialized = true;
        }

        private void ToggleLevelSelection(bool on)
        {
            startScreenGO.SetActive(!on);
            levelSelectionGO.SetActive(on);
        }
    }
}