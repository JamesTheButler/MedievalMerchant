using Common.Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.GameModifiers.UI
{
    public sealed class InGameLevelConditionsPanelHandler : MonoBehaviour
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

            panel.SetActive(!isActive);
        }

        private void Show()
        {
            levelConditionsElement.Setup(GlobalContext.CurrentLevelInfo?.GameplayModifiers);
            gameEventListUI.Bind();
        }

        private void Hide()
        {
            gameEventListUI.Unbind();
        }
    }
}