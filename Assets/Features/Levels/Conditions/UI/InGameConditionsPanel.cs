using Common.Infrastructure;
using Common.Infrastructure.Gameplay;
using Common.UI.Elements;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.Conditions.UI
{
    public sealed class InGameConditionsPanel : DynamicPanel
    {
        [SerializeField, Required]
        private InGameConditionListUI winConditionListUI;

        [SerializeField, Required]
        private InGameConditionListUI lossConditionListUI;

        public override void Initialize()
        {
            base.Initialize();
            var conditions = GameplayContext.Instance.Model.Conditions;

            winConditionListUI.Setup(conditions.WinConditions);
            lossConditionListUI.Setup(conditions.LossConditions);

            Close();
        }

        protected override void OnOpen()
        {
            gameObject.SetActive(true);
        }

        protected override void OnClose()
        {
            gameObject.SetActive(false);
        }
    }
}