using System.Linq;
using Common.Infrastructure;
using Common.Infrastructure.Global;
using Common.UI.Elements;
using Common.Utility;
using Features.Audio.Music;
using Features.Feedback.UI;
using Features.Levels;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Features.StartMenu.UI
{
    public sealed class StartMenuManager : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject startScreenGO, levelSelectionGO, tutorialPopupGO;

        [SerializeField, Required]
        private LevelLoader levelLoader;

        [SerializeField, Required]
        private LevelInfoBox levelInfoBox;

        [SerializeField, Required]
        private FeedbackForm feedbackForm;

        [SerializeField, Required]
        private LevelInfo tutorialLevelInfo;

        private bool _initialized;
        private LevelButton[] _levelButtons;

        public void LoadTutorial()
        {
            levelLoader.LoadLevel(tutorialLevelInfo);
        }

        private void Start()
        {
            startScreenGO.SetActive(true);

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

            InitializeEverything();

            GlobalContext.Instance.Services.MusicService.MusicModeChange.Invoke(MusicMode.Menu);
        }

        private static void InitializeEverything()
        {
            var behaviors = FindObjectsByType<InitializableBehavior>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var behavior in behaviors)
            {
                behavior.Initialize();
            }

            var singletons = FindObjectsByType<InitializableSingleton>(FindObjectsInactive.Include, FindObjectsSortMode.None);
            foreach (var singleton in singletons)
            {
                singleton.Initialize();
            }
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

            ToggleLevelSelection(true);

            _initialized = true;
        }

        private void ToggleLevelSelection(bool on)
        {
            startScreenGO.SetActive(!on);
            levelSelectionGO.SetActive(on);

            var tutorialService = GlobalContext.Instance.Services.TutorialService;
            tutorialPopupGO.SetActive(!tutorialService.HasCompletedIntro);
            tutorialService.SetIntroCompleted();
        }
    }
}