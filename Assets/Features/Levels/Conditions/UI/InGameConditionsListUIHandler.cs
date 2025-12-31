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

        public void Toggle()
        {
            winConditionsUi.SetActive(!winConditionsUi.activeSelf);
        }

        public void Show()
        {
            winConditionsUi.SetActive(true);
        }

        public void Close()
        {
            winConditionsUi.SetActive(false);
        }

        private void Start()
        {
            Initialize();
        }

        private void Initialize()
        {
            var conditions = GameplayContext.Instance.Model.Conditions;

            winConditionListUI.Setup(conditions.WinConditions);
            lossConditionListUI.Setup(conditions.LossConditions);

            winConditionsUi.SetActive(false);
        }
    }
}