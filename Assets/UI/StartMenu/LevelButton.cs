using Features.Levels.Config;
using Infrastructure;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace UI.StartMenu
{
    public sealed class LevelButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField]
        private UnityEvent<LevelInfo> mouseEnter;

        [SerializeField]
        private UnityEvent mouseExit;

        [SerializeField, Required]
        private StartMenuManager startMenuManager;

        [SerializeField, Required]
        private Button button;

        [SerializeField, Required]
        private Image completeIcon;

        [SerializeField, Required]
        private TMP_Text label;

        [SerializeField]
        private LevelInfo levelInfo;

        private void Start()
        {
            if (levelInfo == null || !levelInfo.IsEnabled)
            {
                button.interactable = false;
            }

            button.onClick.AddListener(OnClick);
            label.text = levelInfo.DisplayIndex.ToString();
            var isCompleted = GlobalContext.ProgressModel.CompletedLevels[levelInfo.InternalIndex] != null;
            completeIcon.gameObject.SetActive(isCompleted);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        private void OnClick()
        {
            startMenuManager.LoadLevel(levelInfo);
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (!levelInfo) return;
            mouseEnter.Invoke(levelInfo);
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (!levelInfo) return;
            mouseExit.Invoke();
        }
    }
}