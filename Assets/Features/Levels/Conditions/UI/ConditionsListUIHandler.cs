using Common.Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace Features.Levels.Conditions.UI
{
    public sealed class ConditionsListUIHandler : MonoBehaviour
    {
        [SerializeField, Required]
        private GameObject winConditionsUi;

        [SerializeField, Required]
        private ConditionListUI winConditionListUI;

        [SerializeField, Required]
        private ConditionListUI lossConditionListUI;

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