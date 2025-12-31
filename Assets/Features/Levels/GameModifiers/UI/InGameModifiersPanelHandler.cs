using System;
using Common.Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.GameModifiers.UI
{
    public sealed class InGameModifiersPanelHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject panel;

        [SerializeField, Required]
        private GameModifierUIElement levelConditionsElement;

        [SerializeField, Required]
        private GameEventListUI gameEventListUI;

        public void Toggle()
        {
            var isActive = panel.activeSelf;

            if (isActive)
            {
                Hide();
            }
            else
            {
                Show();
            }
        }

        public void Show()
        {
            if (panel.activeSelf)
                return;

            levelConditionsElement.Setup(GlobalContext.CurrentLevelInfo?.GameplayModifiers);
            gameEventListUI.Bind();
            panel.SetActive(true);
        }

        public void Hide()
        {
            if (!panel.activeSelf)
                return;

            gameEventListUI.Unbind();
            panel.SetActive(false);
        }
    }
}