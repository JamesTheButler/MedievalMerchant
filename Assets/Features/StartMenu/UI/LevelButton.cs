using System;
using Features.Levels.Config;
using Infrastructure;
using NaughtyAttributes;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Features.StartMenu.UI
{
    public sealed class LevelButton : MonoBehaviour
    {
        public event Action<LevelButton> Clicked;

        [SerializeField, Required]
        private Button button;

        [SerializeField, Required]
        private Image completeIcon, lockedIcon;

        [SerializeField, Required]
        private TMP_Text numberText, nameText;

        [field: SerializeField, Expandable, Required]
        public LevelInfo LevelInfo { get; private set; }

        [SerializeField]
        private Color selectedColor;

        private Color _defaultColor;

        private void Start()
        {
            _defaultColor = button.GetComponent<Image>().color;
            var isEnabled = LevelInfo != null && LevelInfo.IsEnabled;
            button.interactable = isEnabled;
            lockedIcon.gameObject.SetActive(!isEnabled);

            button.onClick.AddListener(OnClick);
            numberText.text = LevelInfo.LevelNumberText;
            nameText.text = LevelInfo.LevelName;
            var isCompleted = GlobalContext.Instance.ProgressModel.CompletedLevels[LevelInfo.InternalIndex] != null;
            completeIcon.gameObject.SetActive(isCompleted);
        }

        private void OnDestroy()
        {
            button.onClick.RemoveListener(OnClick);
        }

        public void Select()
        {
            button.GetComponent<Image>().color = selectedColor;
        }

        public void Deselect()
        {
            button.GetComponent<Image>().color = _defaultColor;
        }

        private void OnClick()
        {
            Clicked?.Invoke(this);
            Select();
        }
    }
}