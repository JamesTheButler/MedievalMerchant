using Data;
using NaughtyAttributes;
using UnityEngine;

namespace UI
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
            var conditionsManager = Model.Instance.ConditionManager;

            winConditionListUI.Setup(conditionsManager.WinConditions);
            lossConditionListUI.Setup(conditionsManager.LossConditions);

            winConditionsUi.SetActive(false);
        }

        public void Toggle()
        {
            winConditionsUi.SetActive(!winConditionsUi.activeSelf);
        }
    }
}