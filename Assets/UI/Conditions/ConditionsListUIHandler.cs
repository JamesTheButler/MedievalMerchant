using Infrastructure;
using NaughtyAttributes;
using UnityEngine;

namespace UI.Conditions
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
            var conditionsManager = GameplayContext.Instance.Systems.LevelConditionManager;

            winConditionListUI.Setup(conditionsManager.WinConditions, true);
            lossConditionListUI.Setup(conditionsManager.LossConditions, true);

            winConditionsUi.SetActive(false);
        }

        public void Toggle()
        {
            winConditionsUi.SetActive(!winConditionsUi.activeSelf);
        }
    }
}