using Common.Infrastructure.Global;
using Common.UI.Elements;
using Common.UI.Elements.Panels;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.GameModifiers.UI
{
    public sealed class InGameModifiersPanel : DynamicPanel
    {
        [SerializeField, Required]
        private GameModifierUIElement levelConditionsElement;

        [SerializeField, Required]
        private GameEventListUI gameEventListUI;

        public override void Initialize()
        {
            base.Initialize();
            levelConditionsElement.Setup(GlobalContext.CurrentLevelInfo?.GameplayModifiers);
            Close();
        }

        protected override void OnOpen()
        {
            gameEventListUI.Bind();
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameEventListUI.Unbind();
            gameObject.SetActive(false);
        }
    }
}