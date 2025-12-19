using Common.Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.Conditions.UI
{
    public sealed class InGameConditionsListUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject winConditionsUi;

        [SerializeField, Required]
        private InGameConditionListUI winConditionListUI;

        [SerializeField, Required]
        private InGameConditionListUI lossConditionListUI;

        public void Initialize()
        {
            var conditions = GameplayContext.Instance.Model.Conditions;

            winConditionListUI.Setup(conditions.WinConditions);
            lossConditionListUI.Setup(conditions.LossConditions);

            winConditionsUi.SetActive(false);
        }

        public void Toggle()
        {
            winConditionsUi.SetActive(!winConditionsUi.activeSelf);
        }
    }
}