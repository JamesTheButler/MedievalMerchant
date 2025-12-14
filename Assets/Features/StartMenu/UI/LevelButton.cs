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
        public event Action<LevelInfo> Clicked;

        [SerializeField, Required]
        private Button button;

        [SerializeField, Required]
        private Image completeIcon, lockedIcon;

        [SerializeField, Required]
        private TMP_Text numberText, nameText;

        [field: SerializeField, Expandable, Required]
        public LevelInfo LevelInfo { get; private set; }

        private void Start()
        {
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

        private void OnClick()
        {
            Clicked?.Invoke(LevelInfo);
        }
    }
}