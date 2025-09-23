using Levels;
using NaughtyAttributes;
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