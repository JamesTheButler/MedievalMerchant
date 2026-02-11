using System;
using Common.Infrastructure.Global;
using Common.UI.Tooltips;
using Common.Utility;
using Features.Levels;
using NaughtyAttributes;
using UnityEngine;
using UnityEngine.Localization;
using UnityEngine.Localization.Components;
using UnityEngine.Serialization;
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
        private LocalizeStringEvent levelIndexText, titleText;
        
        [field: SerializeField, Expandable, Required]
        public LevelInfo LevelInfo { get; private set; }

        [SerializeField]
        private Color defaultColor, selectedColor;

        [SerializeField, Required]
        private SimpleTooltipHandler tooltip;

        private void Start()
        {
            var isEnabled = LevelInfo != null && LevelInfo.IsEnabled;
            button.interactable = isEnabled;
            lockedIcon.gameObject.SetActive(!isEnabled);
            tooltip.SetEnabled(!isEnabled);

            button.onClick.AddListener(OnClick);

            levelIndexText.SetArguments(LevelInfo.DisplayIndex);
            titleText.StringReference = LevelInfo.LevelName;
            var progressModel = GlobalContext.Instance.Model.ProgressModel;
            var isCompleted = progressModel.CompletedLevels[LevelInfo.InternalIndex] != null;
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
            button.GetComponent<Image>().color = defaultColor;
        }

        private void OnClick()
        {
            Clicked?.Invoke(this);
            Select();
        }
    }
}